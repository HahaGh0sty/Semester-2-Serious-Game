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
    private int maxWaterTiles;
    private int waterTileCount = 0;

    void Start()
    {
        Camera mainCamera = Camera.main;
        if (tiles.Length == 0 || specialTile == null || borderTile == null || mainCamera == null || waterTile == null || grassTile == null) return;

        tileSize = GetTileSize(tiles[0]);
        maxWaterTiles = Mathf.FloorToInt((gridSize * gridSize) * 0.1f); // Limit water tiles to 10%
        GenerateMap();
    }

    void GenerateMap()
    {

        // Find center region
        int halfSize = gridSize / 2;
        List<Vector2Int> centerTiles = new List<Vector2Int>
{
    new Vector2Int(halfSize, halfSize),
    new Vector2Int(halfSize - 1, halfSize),
    new Vector2Int(halfSize, halfSize - 1),
    new Vector2Int(halfSize - 1, halfSize - 1)
};

        // Pick a random center tile for the special tile
        Vector2Int specialTileGridPos = centerTiles[Random.Range(0, centerTiles.Count)];
        Vector3 specialTilePosition = new Vector3(specialTileGridPos.x * tileSize, -specialTileGridPos.y * tileSize, 0);
        placedTiles[specialTileGridPos] = Instantiate(specialTile, specialTilePosition, Quaternion.identity);

        // Generate border around the special tile
        GenerateBordersAroundSpecialTile(specialTileGridPos);

        List<GameObject> tileList = new List<GameObject>(tiles);
        Shuffle(tileList); // Randomize the normal tiles


        placedTiles[specialTileGridPos] = Instantiate(specialTile, specialTilePosition, Quaternion.identity);

        GenerateBordersAroundSpecialTile(specialTileGridPos);

        Vector3 waterTilePosition = Vector3.zero;
        Vector2Int waterTileGridPos = Vector2Int.zero;
        placedTiles[waterTileGridPos] = Instantiate(waterTile, waterTilePosition, Quaternion.identity);

        GenerateBordersAroundWaterTile(waterTileGridPos);

        float gridOffset = (gridSize / 2) * tileSize;
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int position = new Vector2Int(x - gridSize / 2, y - gridSize / 2);
                if (position == specialTileGridPos || placedTiles.ContainsKey(position)) continue;
                allPositions.Add(position);
            }
        }

        Shuffle(allPositions);

        foreach (Vector2Int position in allPositions)
        {
            if (placedTiles.ContainsKey(position)) continue;

            GameObject selectedTile = tileList[Random.Range(0, tileList.Count)];

            if (selectedTile == waterTile && waterTileCount >= maxWaterTiles)
            {
                selectedTile = grassTile; // Replace excess water tiles with grass
            }
            else if (selectedTile == waterTile)
            {
                waterTileCount++;
            }

            Vector3 worldPosition = new Vector3(position.x * tileSize, -position.y * tileSize, 0);
            placedTiles[position] = Instantiate(selectedTile, worldPosition, Quaternion.identity);
        }

        EnsureSeparation();
        SetCameraPosition(specialTilePosition);
    }

    void GenerateBordersAroundWaterTile(Vector2Int waterTilePos)
    {
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),
                                    new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)};

        foreach (Vector2Int dir in directions)
        {
            Vector2Int borderPos = waterTilePos + dir;
            if (!placedTiles.ContainsKey(borderPos))
            {
                Vector3 worldPos = new Vector3(borderPos.x * tileSize, -borderPos.y * tileSize, 0);
                placedTiles[borderPos] = Instantiate(separatingTile, worldPos, Quaternion.identity);
            }
        }
    }

    void GenerateBordersAroundSpecialTile(Vector2Int specialTilePos)
    {
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),
                                    new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)};

        foreach (Vector2Int dir in directions)
        {
            Vector2Int borderPos = specialTilePos + dir;
            if (!placedTiles.ContainsKey(borderPos))
            {
                Vector3 worldPos = new Vector3(borderPos.x * tileSize, -borderPos.y * tileSize, 0);
                placedTiles[borderPos] = Instantiate(borderTile, worldPos, Quaternion.identity);
            }
        }
    }



    void EnsureSeparation()
    {
        List<Vector2Int> waterPositions = new List<Vector2Int>();

        foreach (var pos in placedTiles.Keys)
        {
            if (placedTiles[pos].name.Contains("Water"))
            {
                waterPositions.Add(pos);
            }
        }

        foreach (var pos in waterPositions)
        {
            Vector2Int[] directions = { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = pos + dir;

                // Ensure it's inside the map bounds
                if (neighborPos.x < -gridSize / 2 || neighborPos.x >= gridSize / 2 ||
                    neighborPos.y < -gridSize / 2 || neighborPos.y >= gridSize / 2)
                {
                    continue;
                }

                // Check if it's a Grass tile and place a separator
                if (!placedTiles.ContainsKey(neighborPos) || placedTiles[neighborPos].name.Contains("Grass"))
                {
                    Vector3 worldPos = new Vector3(neighborPos.x * tileSize, -neighborPos.y * tileSize, 0);
                    GameObject separator = Instantiate(separatingTile, worldPos, GetCorrectRotation(dir));
                    placedTiles[neighborPos] = separator;
                }
            }
        }
    }


    Quaternion GetCorrectRotation(Vector2Int dir)
    {
        if (dir == new Vector2Int(1, 0)) return Quaternion.Euler(0, 0, -90); // Water on the right
        if (dir == new Vector2Int(0, 1)) return Quaternion.Euler(0, 0, 180);  // Water on the top
        if (dir == new Vector2Int(-1, 0)) return Quaternion.Euler(0, 0, 90); // Water on the left
        if (dir == new Vector2Int(0, -1)) return Quaternion.Euler(0, 0, 0); // Water on the bottom
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
