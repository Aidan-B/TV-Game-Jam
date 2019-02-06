using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileMapGeneration : MonoBehaviour
{
    public Tile tile;
    Tilemap tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Tilemap>();
        tm.SetTile(new Vector3Int(0, 0, 0), tile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
