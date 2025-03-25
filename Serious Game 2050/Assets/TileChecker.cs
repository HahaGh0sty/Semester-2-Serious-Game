using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class TileChecker : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] grassTiles, waterTiles, forestTiles;

    private HashSet<TileBase> grassTilesSet;
    private HashSet<TileBase> waterTilesSet;
    private HashSet<TileBase> forestTilesSet;
    private Vector3Int lastCheckedPosition;

    public bool tileMapGrassActive = false;
    public bool tileMapForestActive = false;
    public bool tileMapWaterActive = false;

    void Start()
    {
        grassTilesSet = new HashSet<TileBase>(grassTiles);
        waterTilesSet = new HashSet<TileBase>(waterTiles);
        forestTilesSet = new HashSet<TileBase>(forestTiles);

        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>(); // Automatically finds the first Tilemap in the scene
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found! Make sure there is a Tilemap in the scene.");
        }
    }

    void Update()
    {
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        if (currentCell != lastCheckedPosition) // Only check when position changes
        {
            lastCheckedPosition = currentCell;
            CheckTile(currentCell);
        }
    }

    void CheckTile(Vector3Int cellPosition)
    {
        TileBase tile = tilemap.GetTile(cellPosition);
        if (tile == null) return; // No tile, no need to check

        if (grassTiles.Contains(tile))
        {
            tileMapGrassActive = true;
            tileMapForestActive = false;
            tileMapWaterActive = false;
            Debug.Log("Grass Tile detected!");
        }
        else if (waterTiles.Contains(tile))
        {
            tileMapWaterActive = true;
            tileMapGrassActive = false;
            tileMapForestActive = false;
            Debug.Log("Water Tile detected!");
        }
        else if (forestTiles.Contains(tile))
        {
            tileMapForestActive = true;
            tileMapGrassActive = false;
            tileMapWaterActive = false;
            Debug.Log("Forest Tile detected!");
        }
    }
}
