using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class TileChecker : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] grassTiles, waterTiles, forestTiles, buildingTiles;

    private HashSet<TileBase> grassTilesSet;
    private HashSet<TileBase> waterTilesSet;
    private HashSet<TileBase> forestTilesSet;
    private HashSet<TileBase> buildingTilesSet;
    private Vector3Int lastCheckedPosition;

    public bool tileMapGrassActive = false;
    public bool tileMapForestActive = false;
    public bool tileMapWaterActive = false;
    public bool tileMapBuildingActive = false;


    [SerializeField] private GameObject ThisGrid;
    [SerializeField] public bool Selected = false;
    [SerializeField] public bool Empty = false;

    [SerializeField] Color s_MouseOverColor;
    [SerializeField] Color s_MouseOverColor2;
    [SerializeField] Color s_IsCurrentOriginalColor;
    [SerializeField] Color s_IsGrassTileColor;
    [SerializeField] Color s_IsForestTileColor;
    [SerializeField] Color s_IsWaterTileColor;
    [SerializeField] SpriteRenderer s_Renderer;

    void Start()
    {
        grassTilesSet = new HashSet<TileBase>(grassTiles);
        waterTilesSet = new HashSet<TileBase>(waterTiles);
        forestTilesSet = new HashSet<TileBase>(forestTiles);
        buildingTilesSet = new HashSet<TileBase>(buildingTiles);


        ThisGrid = transform.gameObject;
        s_MouseOverColor = new Color(1, 0, 0, 0.8f);
        s_IsGrassTileColor = new Color(0.5136409f, 1, 0, 0.8f);
        s_IsForestTileColor = new Color(0.06611012f, 0.6226415f, 0, 0.8f);
        s_IsWaterTileColor = new Color(0, 0.184f, 1, 0.8f);
        s_Renderer = GetComponent<SpriteRenderer>();

        s_MouseOverColor2 = new Color(1, 0.5f, 0.5f, 0.5f);

        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>(); // Automatically finds the first Tilemap in the scene
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found! Make sure there is a Tilemap in the scene.");
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
            s_Renderer.material.color = s_IsGrassTileColor;
            s_IsCurrentOriginalColor = s_IsGrassTileColor;
            Debug.Log("Grass Tile detected on " + transform.name + "!");
        }
        else if (waterTiles.Contains(tile))
        {
            tileMapWaterActive = true;
            tileMapGrassActive = false;
            tileMapForestActive = false;
            s_Renderer.material.color = s_IsWaterTileColor;
            s_IsCurrentOriginalColor = s_IsWaterTileColor;
            Debug.Log("Water Tile detected on " + transform.name + "!");
        }
        else if (forestTiles.Contains(tile))
        {
            tileMapForestActive = true;
            tileMapGrassActive = false;
            tileMapWaterActive = false;
            s_Renderer.material.color = s_IsForestTileColor;
            s_IsCurrentOriginalColor = s_IsForestTileColor;
            Debug.Log("Forest Tile detected on " + transform.name + "!");
        }
        else if (buildingTiles.Contains(tile))
        {
            
        }
    }

    //GridSelect script merge//
    private void OnMouseOver()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_MouseOverColor2;
        }

        if (Selected == true)
        {
            s_Renderer.material.color = s_MouseOverColor2;
        }
    }

    private void OnMouseExit()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_IsCurrentOriginalColor;
        }

        if (Selected == true)
        {
            s_Renderer.material.color = s_MouseOverColor;
        }
    }

    private void OnMouseDown()
    {
        if (Selected == false)
        {
            Selected = true;
        }
        else if (Selected == true)
        {
            Selected = false;
        }
    }
}
