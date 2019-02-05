using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public GameObject square;

    public const int Width = 500;
    public const int Height = 500;
    public const int WalkerPaths = 500;
    [Range(0, 1)]public float xBias = 0.5f;
    [Range(0, 1)] public float yBias = 0.25f;


    private mapTile[,] map = new mapTile[Width, Height];
    private bool[,] mapPaths = new bool[Width, Height];
    private Vector2[,] directions = new Vector2[2,2] { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    // Start is called before the first frame update
    void Start()
    {
        Vector2 startPos = new Vector2(Random.Range(0, Width), Random.Range(0, Height));
        Vector2 currentPos = startPos;
        Vector2 currentDir = directions[Random.Range(0, 1), Random.Range(0, 1)]; //random direction
        int distance = 0;
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
                    currentDir = directions[1, Random.Range(0, 2)];// left/right
            }
            else
            {
                if (currentPos.y <= 1)
                    currentDir = Vector2.up;
                else if (currentPos.y >= Height - 1)
                    currentDir = Vector2.down;
                else
                    currentDir = directions[0, Random.Range(0, 2)]; // up/down
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
                        Instantiate(square, new Vector3(x, y, 1), Quaternion.identity, gameObject.GetComponent<Transform>());
                    else
                        Instantiate(square, new Vector3(x, y), Quaternion.identity, gameObject.GetComponent<Transform>());
                }
            }
        }
        
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
