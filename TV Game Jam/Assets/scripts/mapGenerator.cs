using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public GameObject square;

    public const int Width = 500;
    public const int Height = 500;
    public int WalkerPaths = 2000;
    [Range(0, 1)]public float xBias = 0.1f; //longer horizontal tunnels
    [Range(0, 1)] public float yBias = 0.01f; //longer vertical tunnels
    [Range(0.5f, 2f)] public float RightLeftBias = 2f; // >1 is right, <1 is left
    [Range(0.5f, 2f)] public float DownUpBias = 1.5f; // >1 is down, <1 is up


    private mapTile[,] map = new mapTile[Width, Height];
    private bool[,] mapPaths = new bool[Width, Height];
    private Vector2[,] directions = new Vector2[2,2] { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    // Start is called before the first frame update
    void Start()
    {
        Vector2 startPos = new Vector2(Mathf.RoundToInt(Width * 0.5f), Mathf.RoundToInt(Height * 0.5f));
        Vector2 currentPos = startPos;
        Vector2 currentDir = directions[Random.Range(0, 1), Random.Range(0, 1)]; //random direction
        int distance = 0;
        int roomCounter = 0;
        mapPaths[(int)currentPos.x, (int)currentPos.y] = true;
        for (int moves = 0; moves < WalkerPaths; moves++)
        {
            //Change Direction, move away from map bounds if near it
            if (currentDir == Vector2.up || currentDir == Vector2.down)
            {
                if (currentPos.x <= 1)
                    currentDir = Vector2.right;
                else if (currentPos.x >= Width - 1)
                    currentDir = Vector2.left;
                else
                    currentDir = directions[1, Mathf.RoundToInt(Mathf.Clamp( Random.Range(0f, 1f) * RightLeftBias, 0f, 1f))];// left/right
            }
            else
            {
                if (currentPos.y <= 1)
                    currentDir = Vector2.up;
                else if (currentPos.y >= Height - 1)
                    currentDir = Vector2.down;
                else
                    currentDir = directions[0, Mathf.RoundToInt(Mathf.Clamp(Random.Range(0f, 1f) * DownUpBias, 0f, 1f))]; // up/down
            }
            

            if (currentDir == Vector2.up)
            {
                distance = Random.Range(1, (int)((Height - currentPos.y)  * yBias));
            }
            else if (currentDir == Vector2.down)
            {
                distance = Random.Range(1, (int)(currentPos.y * yBias));
            }
            else if (currentDir == Vector2.left)
            {
                distance = Random.Range(1, (int)(currentPos.x * xBias));
            }
            else if (currentDir == Vector2.right)
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

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (mapPaths[x,y])
                {
                    if (startPos == new Vector2(x,y))
                        Instantiate(square, new Vector3(x, y, 10), Quaternion.identity, gameObject.GetComponent<Transform>());
                    else
                        Instantiate(square, new Vector3(x, y), Quaternion.identity, gameObject.GetComponent<Transform>());
                    roomCounter++;
                }
            }
        }
        Debug.Log(roomCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
public struct mapTile
{
    public Vector2 coords;
    public bool up, down, left, right;

    public mapTile(Vector2 coords, bool up, bool down, bool left, bool right)
    {
        this.coords = coords;
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
    }

}
