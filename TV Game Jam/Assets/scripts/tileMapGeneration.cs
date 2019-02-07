using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileMapGeneration : MonoBehaviour
{
    public RuleTile tile;
    public Vector2Int size = new Vector2Int(10, 10);
    Tilemap tm;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (x == 0 || y == 0 || x == size.x-1 || y == size.y - 1)
                {
                    tm = GetComponent<Tilemap>();
                    tm.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
        
    }
    
}
