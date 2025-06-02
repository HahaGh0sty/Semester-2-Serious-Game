using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GhostBuildingDetectTile : MonoBehaviour
{
    public Tilemap tilemap;

    [SerializeField] private string LastTile;
    public TileBase[] grassTiles, waterTiles, forestTiles, buildingTiles;

    private HashSet<TileBase> grassTilesSet;
    private HashSet<TileBase> waterTilesSet;
    private HashSet<TileBase> forestTilesSet;
    private HashSet<TileBase> buildingTilesSet;
    private Vector3Int lastCheckedPosition;

    public IsFucking IsFucking;

    void Start()
    {
        IsFucking = FindObjectOfType<IsFucking>();

        grassTilesSet = new HashSet<TileBase>(grassTiles);
        waterTilesSet = new HashSet<TileBase>(waterTiles);
        forestTilesSet = new HashSet<TileBase>(forestTiles);
        buildingTilesSet = new HashSet<TileBase>(buildingTiles);

        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>(); // Automatically finds the first Tilemap in the scene
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found! Make sure there is a T]ilemap in the scene.");
        }
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        lastCheckedPosition = currentCell;
        CheckTile(currentCell);

    }

    void Update()
    {
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        if (currentCell != lastCheckedPosition) // Only check when position changes
        {
            if (LastTile == "Grass Tile")
            {
                IsFucking.RemoveGrassTileCount();
            }
            if (LastTile == "Forest Tile")
            {
                IsFucking.RemoveForestTilecount();
            }
            if (LastTile == "Water Tile")
            {
                IsFucking.RemoveWaterTileCount();
            }
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
            gameObject.tag = "IsGrassTile";
            LastTile = "Grass Tile";
            IsFucking.AddGrassTileCount();
            //Debug.Log("Grass Tile detected on " + transform.name + "!");
        }
        else if (waterTiles.Contains(tile))
        {
            gameObject.tag = "IsWaterTile";
            LastTile = "Water Tile";
            IsFucking.AddWaterTileCount();
            //Debug.Log("Water Tile detected on " + transform.name + "!");
        }
        else if (forestTiles.Contains(tile))
        {
            gameObject.tag = "IsForestTile";
            LastTile = "Forest Tile";
            IsFucking.AddForestTileCount();
            //Debug.Log("Forest Tile detected on " + transform.name + "!");
        }

        //else if (buildingTiles.Contains(tile))
        //{
        //    gameObject.tag = "IsOnBuilding";
        //}
                //       IMPORTANT FOR LATER: VRAAG LUC OF DE BUILDINGS OP DE TILEMAP TELLEN ALS EEN DETECTABLE TILE OF NIET


                // if YES - add deze lijn en verwijder alle collisions op NORMALE BUILDINGS
                // if NO - verwijder deze notes
    }
}