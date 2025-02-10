using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tiles; // Normal tile prefabs
    public GameObject specialTile; // The unique tile that appears once
    public GameObject borderTile; // Tile that surrounds the special tile
    public GameObject waterTile; // Water tile prefab
    public GameObject grassTile; // Grass tile prefab
    public GameObject separatingTile; // "Grass/Water" separating tile
    public int gridSize = 5; // Grid size (e.g., 5x5)
    public Camera mainCamera; // Reference to the camera
    private float tileSize;
    private Dictionary<Vector2Int, GameObject> placedTiles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        if (tiles.Length == 0 || specialTile == null || borderTile == null || mainCamera == null || waterTile == null || grassTile == null || separatingTile == null) return;

        tileSize = GetTileSize(tiles[0]);
        GenerateMap();
    }

    void GenerateMap()
    {
        List<GameObject> tileList = new List<GameObject>(tiles);
        tileList.Add(waterTile);
        tileList.Add(grassTile);
        Shuffle(tileList); // Randomize the normal tiles

        Vector3 specialTilePosition = Vector3.zero;
        Vector2Int specialTileGridPos = Vector2Int.zero;
        placedTiles[specialTileGridPos] = Instantiate(specialTile, specialTilePosition, Quaternion.identity);

        float gridOffset = (gridSize / 2) * tileSize;
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int position = new Vector2Int(x - gridSize / 2, y - gridSize / 2);
                if (position == specialTileGridPos) continue;
                allPositions.Add(position);
            }
        }

        Shuffle(allPositions);

        foreach (Vector2Int position in allPositions)
        {
            if (placedTiles.ContainsKey(position)) continue;

            GameObject selectedTile = tileList[Random.Range(0, tileList.Count)];

            if (selectedTile == waterTile)
            {
                if (!HasEnoughWaterNeighbors(position))
                {
                    selectedTile = grassTile;
                }
            }

            Vector3 worldPosition = new Vector3(position.x * tileSize, -position.y * tileSize, 0);
            placedTiles[position] = Instantiate(selectedTile, worldPosition, Quaternion.identity);
        }

        EnsureSeparation();
        GenerateBorders();
        SetCameraPosition(specialTilePosition);
    }

    bool HasEnoughWaterNeighbors(Vector2Int position)
    {
        int waterCount = 0;
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (placedTiles.ContainsKey(neighborPos) && placedTiles[neighborPos] == waterTile)
            {
                waterCount++;
            }
        }
        return waterCount >= 2;
    }

    void EnsureSeparation()
    {
        foreach (var pos in new List<Vector2Int>(placedTiles.Keys))
        {
            if (placedTiles[pos] == waterTile)
            {
                Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighborPos = pos + dir;
                    if (placedTiles.ContainsKey(neighborPos) && placedTiles[neighborPos] == grassTile)
                    {
                        Vector3 worldPos = new Vector3(neighborPos.x * tileSize, -neighborPos.y * tileSize, 0);
                        GameObject separator = Instantiate(separatingTile, worldPos, GetCorrectRotation(dir));
                        placedTiles[neighborPos] = separator;
                    }
                }
            }
        }
    }

    void GenerateBorders()
    {
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int borderPos = dir;
            if (!placedTiles.ContainsKey(borderPos))
            {
                Vector3 worldPos = new Vector3(borderPos.x * tileSize, -borderPos.y * tileSize, 0);
                placedTiles[borderPos] = Instantiate(borderTile, worldPos, Quaternion.identity);
            }
        }
    }

    Quaternion GetCorrectRotation(Vector2Int dir)
    {
        if (dir == new Vector2Int(1, 0)) return Quaternion.Euler(0, 0, 90); // Water on the right
        if (dir == new Vector2Int(0, 1)) return Quaternion.Euler(0, 0, 0);  // Water on the top
        if (dir == new Vector2Int(-1, 0)) return Quaternion.Euler(0, 0, -90); // Water on the left
        if (dir == new Vector2Int(0, -1)) return Quaternion.Euler(0, 0, 180); // Water on the bottom
        return Quaternion.identity;
    }

    void SetCameraPosition(Vector3 specialTilePosition)
    {
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
