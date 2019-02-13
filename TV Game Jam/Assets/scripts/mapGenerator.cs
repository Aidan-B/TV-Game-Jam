using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class Room
{
    public List<GameObject> list = new List<GameObject>() { null };
}
 
 

public class mapGenerator : MonoBehaviour
{
    public RuleTile tile;
    public Vector2Int roomSize = new Vector2Int(10, 10);
    
    public Tilemap walls;
    public Tilemap platform;
    public Tilemap stairRight;
    public Tilemap stairLeft;

    Tilemap thisWall, thisPlatform, thisStairR, thisStairL;
    GameObject objects;

    public const int Width = 500;
    public const int Height = 500;

    public GameObject player;

    public int WalkerPaths = 50;
    public Room[] rooms = new Room[16];
    //public List<GameObject>[] rooms = new List<GameObject>[15];
    //public List<roomList>[] room;
    [Range(0, 1)]public float xBias = 0.1f; //longer horizontal tunnels
    [Range(0, 1)] public float yBias = 0.01f; //longer vertical tunnels
    [Range(0.5f, 2f)] public float RightLeftBias = 1.5f; // >1 is right, <1 is left
    [Range(0.5f, 2f)] public float DownUpBias = 1.5f; // >1 is down, <1 is up

    private mapTile[,] map = new mapTile[Width, Height];
    public bool[,] mapPaths = new bool[Width, Height];
    private Vector2Int[,] directions = new Vector2Int[2,2] { { Vector2Int.up, Vector2Int.down }, { Vector2Int.left, Vector2Int.right } };
    //public Vector2Int tileDimentions = new Vector2Int(8, 6);

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(Width * 0.5f), Mathf.RoundToInt(Height * 0.5f));
        Vector2Int currentPos = startPos;
        Vector2Int currentDir = directions[Random.Range(0, 1), Random.Range(0, 1)]; //random direction
        int distance = 0;
        int roomCounter = 0;
        mapPaths[(int)currentPos.x, (int)currentPos.y] = true;
        for (int moves = 0; moves < WalkerPaths; moves++)
        {
            //Change Direction, move away from map bounds if near it
            if (currentDir == Vector2Int.up || currentDir == Vector2Int.down)
            {
                if (currentPos.x <= 1)
                    currentDir = Vector2Int.right;
                else if (currentPos.x >= Width - 1)
                    currentDir = Vector2Int.left;
                else
                    currentDir = directions[1, Mathf.RoundToInt(Mathf.Clamp( Random.Range(0f, 1f) * RightLeftBias, 0f, 1f))];// left/right
            }
            else
            {
                if (currentPos.y <= 1)
                    currentDir = Vector2Int.up;
                else if (currentPos.y >= Height - 1)
                    currentDir = Vector2Int.down;
                else
                    currentDir = directions[0, Mathf.RoundToInt(Mathf.Clamp(Random.Range(0f, 1f) * DownUpBias, 0f, 1f))]; // up/down
            }
            

            if (currentDir == Vector2Int.up)
            {
                distance = Random.Range(1, (int)((Height - currentPos.y)  * yBias));
            }
            else if (currentDir == Vector2Int.down)
            {
                distance = Random.Range(1, (int)(currentPos.y * yBias));
            }
            else if (currentDir == Vector2Int.left)
            {
                distance = Random.Range(1, (int)(currentPos.x * xBias));
            }
            else if (currentDir == Vector2Int.right)
            {
                distance = Random.Range(1, (int)((Width - currentPos.x) * xBias));
            }
            else
            {
                Debug.Log("Error");
            }
            //Debug.Log("Distance: " + distance + currentDir);
            for (int step = 0; step < distance; step++)
            {
                currentPos += currentDir;
                //Debug.Log("Position: " + currentPos);
                mapPaths[(int)currentPos.x, (int)currentPos.y] = true;
                //Debug.Log("Current Position: " + currentPos.x + ", " + currentPos.y);
                
            }
        }

        //generate map
        for (int x = 0; x < Width; x++) 
        {
            for (int y = 0; y < Height; y++)
            {
                if (mapPaths[x,y])
                {
                    if (startPos == new Vector2Int(x, y))
                    {
                        //    tile.GetComponent<SpriteRenderer>().color = Color.green;
                        Instantiate(player, new Vector3(roomSize.x * 0.5f, roomSize.y * 0.5f), Quaternion.identity);
                    }

                    int thisRoomShape = new mapTile(mapPaths[x, y + 1], mapPaths[x, y - 1], mapPaths[x - 1, y], mapPaths[x + 1, y]).shape;


                    GameObject thisRoom = rooms[thisRoomShape].list[Random.Range(0, rooms[thisRoomShape].list.Count)];
     
                    foreach (Transform t in thisRoom.transform)
                    {
                        if (t.name == "Walls")
                        {
                            thisWall = t.gameObject.GetComponent<Tilemap>();
                        }
                        else if (t.name == "Platform")
                        {
                            thisPlatform = t.gameObject.GetComponent<Tilemap>();
                        }
                        else if (t.name == "Stairs (Right)")
                        {
                            thisStairR = t.gameObject.GetComponent<Tilemap>();
                        }
                        else if (t.name == "Stairs (Left)")
                        {
                            thisStairL = t.gameObject.GetComponent<Tilemap>();
                        }
                        else if (t.name == "Objects")
                        {
                            objects = t.gameObject;
                            foreach (Transform item in objects.transform) {
                                Instantiate(item.gameObject, new Vector3(x * roomSize.x + item.position.x, y * roomSize.y + item.position.y, 0), item.rotation, transform.Find("Objects"));
                            }
                            //Instantiate(t.gameObject,new Vector3(x * roomSize.x - startPos.x * roomSize.x, y * roomSize.y - startPos.y * roomSize.y, 0), t.rotation, transform);
                        }
                    }

                    //generate room
                    for (int mx = 0; mx < roomSize.x; mx++)
                    {
                        for (int my = 0; my < roomSize.y; my++)
                        {
                            walls.SetTile(new Vector3Int(x * roomSize.x + mx, y * roomSize.y + my, 0), thisWall.GetTile(new Vector3Int(mx, my, 0)));
                            platform.SetTile(new Vector3Int(x * roomSize.x + mx, y * roomSize.y + my, 0), thisPlatform.GetTile(new Vector3Int(mx, my, 0)));
                            stairRight.SetTile(new Vector3Int(x * roomSize.x + mx, y * roomSize.y + my, 0), thisStairR.GetTile(new Vector3Int(mx, my, 0)));
                            stairLeft.SetTile(new Vector3Int(x * roomSize.x + mx, y * roomSize.y + my, 0), thisStairL.GetTile(new Vector3Int(mx, my, 0)));
                        }
                    }
                    roomCounter++;
                }
            }
        }

        walls.CompressBounds();
        platform.CompressBounds();
        stairRight.CompressBounds();
        stairLeft.CompressBounds();
        transform.position = new Vector3(-startPos.x * roomSize.x, -startPos.y * roomSize.y, 0);

        Debug.Log(roomCounter);

        List<Vector2Int> path = GetComponent<pathfinding>().findPath(startPos, currentPos, mapPaths);
        Vector2Int position = startPos;
        foreach (Vector2Int point in path)
        {
            Debug.Log(point);
            Debug.DrawLine(new Vector3(position.x, position.y)*10, new Vector3(point.x, point.y)*10, Color.red);
            position = point;
        }

    }
}
public struct mapTile
{
    //public Vector2Int coords;
    public bool up, down, left, right;
    public readonly int shape;

    //public mapTile(Vector2Int coords, bool up, bool down, bool left, bool right)
    public mapTile(bool up, bool down, bool left, bool right)
    {
        //this.coords = coords;
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;

        if (up && !right && !down && !left)
            shape = 1;
        else if (up && right && !down && !left)
            shape = 2;
        else if (up && right && down && !left)
            shape = 3;
        else if (up && right && !down && left)
            shape = 4;
        else if (up && !right && down && !left)
            shape = 5;
        else if (up && !right && down && left)
            shape = 6;
        else if (up && !right && !down && left)
            shape = 7;
        else if (!up && !right && !down && left)
            shape = 8;
        else if (!up && !right && down && left)
            shape = 9;
        else if (!up && right && down && left)
            shape = 10;
        else if (!up && right && !down && left)
            shape = 11;
        else if (!up && !right && down && !left)
            shape = 12;
        else if (!up && right && down && !left)
            shape = 13;
        else if (!up && right && !down && !left)
            shape = 14;
        else if (up && right && down && left)
            shape = 15;
        else
            shape = 0;
    }

}
