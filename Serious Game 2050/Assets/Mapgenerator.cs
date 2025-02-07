using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tiles; // Normal tile prefabs
    public GameObject specialTile; // The unique tile that appears once
    public GameObject borderTile; // Tile that surrounds the special tile
    public int gridSize = 5; // Grid size (e.g., 5x5)
    public Camera mainCamera; // Reference to the camera
    private float tileSize;

    void Start()
    {
        if (tiles.Length == 0 || specialTile == null || borderTile == null || mainCamera == null) return;

        tileSize = GetTileSize(tiles[0]);
        GenerateMap();
    }

    void GenerateMap()
    {
        List<GameObject> tileList = new List<GameObject>(tiles);
        Shuffle(tileList); // Randomize the normal tiles

        // Special tile is always at (0,0,0)
        Vector3 specialTilePosition = Vector3.zero;
        Instantiate(specialTile, specialTilePosition, Quaternion.identity);

        // Center grid around the special tile
        float gridOffset = (gridSize / 2) * tileSize;

        // Track used positions
        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

        // Place the border tiles around the special tile
        Vector2Int[] borderOffsets =
        {
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
            new Vector2Int(-1,  0),                    new Vector2Int(1,  0),
            new Vector2Int(-1,  1), new Vector2Int(0,  1), new Vector2Int(1,  1)
        };

        foreach (Vector2Int offset in borderOffsets)
        {
            Vector3 worldPosition = new Vector3(offset.x * tileSize, -offset.y * tileSize, 0);
            Instantiate(borderTile, worldPosition, Quaternion.identity);
            occupiedPositions.Add(offset);
        }

        // Place other tiles in the grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int tilePos = new Vector2Int(x - gridSize / 2, y - gridSize / 2);
                if (tilePos == Vector2Int.zero || occupiedPositions.Contains(tilePos)) continue;

                Vector3 worldPosition = new Vector3(tilePos.x * tileSize, -tilePos.y * tileSize, 0);
                GameObject tilePrefab = tileList[Random.Range(0, tileList.Count)];
                Instantiate(tilePrefab, worldPosition, Quaternion.identity);
            }
        }

        // Set camera above special tile
        SetCameraPosition(specialTilePosition);
    }

    void SetCameraPosition(Vector3 specialTilePosition)
    {
        // Camera follows special tile at (0,0)
        mainCamera.transform.position = new Vector3(specialTilePosition.x, specialTilePosition.y, mainCamera.transform.position.z);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    float GetTileSize(GameObject tile)
    {
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : 1.0f;
    }
}
