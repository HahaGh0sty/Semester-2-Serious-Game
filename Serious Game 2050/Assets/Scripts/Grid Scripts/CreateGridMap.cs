using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateGridMap : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase grassTile1;
    public TileBase grassTile2;
    public TileBase grassTile3;
    public TileBase grassTile4;
    public TileBase grassTile5;
    public TileBase forestTile;
    public TileBase grasswaternorthTile;
    public TileBase grasswatereastTile;
    public TileBase grasswatersouthTile;
    public TileBase grasswaterwestTile;
    public TileBase grasswatercornerNWTile;
    public TileBase grasswatercornerNETile;
    public TileBase grasswatercornerSWTile;
    public TileBase grasswatercornerSETile;
    public TileBase grasswatertouchNETile;
    public TileBase grasswatertouchNWTile;
    public TileBase grasswatertouchSETile;
    public TileBase grasswatertouchSWTile;
    public TileBase grasswaterisland;

    public int width = 200;
    public int height = 200;
    public float initialWaterChance = 0.25f;
    public float initialForestChance = 0.25f;
    public int smoothingIterations = 4;
    public int waterClusterSize = 4;
    public int forestClusterSize = 4;



    private TileBase[,] mapData;

    Vector2Int[] directions =
{
    new Vector2Int(1, 0),   // Right
    new Vector2Int(-1, 0),  // Left
    new Vector2Int(0, 1),   // Up
    new Vector2Int(0, -1)   // Down
};


    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Step 1: Start with all grass tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = grassTile;
            }
        }
    }
}