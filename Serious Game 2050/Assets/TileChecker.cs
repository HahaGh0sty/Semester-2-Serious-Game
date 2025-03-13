using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChecker : MonoBehaviour
{
    public Tilemap tileMapGrass;
    public Tilemap tileMapForest;
    public Tilemap tileMapWater;
    public bool tileMapGrassActive;
    public bool tileMapForestActive;
    public bool tileMapWaterActive;
    public TileBase tile;

    private void Start()
    {
        TileMapActive();
        CheckTileUnderneath();
    }

    void TileMapActive()
    {

        if (tileMapGrass || tileMapForest || tileMapWater == null)
        {
            Debug.Log("no tilemap found underneath");
            return;
        }
        if (tileMapGrass)
        {
            if (tileMapForest)
            {
                if (tileMapWater)
                {
                    tileMapWaterActive = true;
                    return;
                }
                tileMapForestActive = true;
                return;
            }
            tileMapGrassActive = true;
            return;
        }
    }
    void CheckTileUnderneath()
    {
        if (tileMapGrass || tileMapForest || tileMapWater == null) return;

        // Convert world position to cell position
        if (tileMapGrassActive == true)
        {
            Vector3Int tilePosition = tileMapGrass.WorldToCell(transform.position);
            TileBase tile = tileMapGrass.GetTile(tilePosition);
        }
        else if (tileMapForestActive == true)
        {
            Vector3Int tilePosition = tileMapForest.WorldToCell(transform.position);
            TileBase tile = tileMapForest.GetTile(tilePosition);
        }
        else if (tileMapWaterActive == true)
        {
            Vector3Int tilePosition = tileMapWater.WorldToCell(transform.position);
            TileBase tile = tileMapWater.GetTile(tilePosition);
        }

        if (tile != null)
        {
            Debug.Log("Tile underneath: " + tile.name);
        }
        else
        {
            Debug.Log("No tile underneath.");
        }
    }
}