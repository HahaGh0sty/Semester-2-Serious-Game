using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilemapInteraction : MonoBehaviour
{
    public Tilemap tilemap;  // Reference to your Tilemap
    public TileBase grassTile;
    public Camera mainCamera; // Camera used to detect mouse position

    public int width = 200;
    public int height = 200;

    private TileBase[,] mapData;

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
        void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left mouse button click
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world space
            Vector3Int cellPos = tilemap.WorldToCell(worldPos); // Convert world position to tilemap cell position

            TileBase clickedTile = tilemap.GetTile(cellPos); // Get the tile at that position

            if (clickedTile != null)
            {
                InteractWithTile(cellPos, clickedTile);  // Call the interaction function
            }
        }
    }

    // A custom interaction function for your tiles
    void InteractWithTile(Vector3Int cellPos, TileBase tile)
    {
        // Example of a simple interaction
        Debug.Log("Interacted with tile at position: " + cellPos);


    }

}
