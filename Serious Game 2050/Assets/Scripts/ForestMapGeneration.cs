using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tilemap;

public class ForestMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] allgrassTiles; // Array for grass tiles
    public TileBase[] allforestTiles; // Array for forest tiles

    private TileBase grassTile; // Default grass tile
    private TileBase forestTile; // Default forest tile

    public int width = 50;
    public int height = 50;
    public float initialWaterChance = 0.25f;
    public float initialForestChance = 0.25f;
    public int smoothingIterations = 4;
    public int waterClusterSize = 4;
    public int forestClusterSize = 4;

    private TileBase[,] mapData;

    private List<TileBase> allowedGrassTiles;
    private List<TileBase> allowedWaterTiles;

    Vector2Int[] directions =
{
    new Vector2Int(1, 0),   // Right
    new Vector2Int(-1, 0),  // Left
    new Vector2Int(0, 1),   // Up
    new Vector2Int(0, -1)   // Down
    };

    private Dictionary<TileBase, float> GrassTileChances;
    private Dictionary<TileBase, float> WaterTileChances;

    TileBase GetRandomTile()
    {
        // Ensure allgrassTiles is not null and has at least one element
        if (allgrassTiles == null || allgrassTiles.Length == 0)
        {
            return null;  // Return a default value or handle the case as needed
        }

        // Return a random tile from allgrassTiles
        return allgrassTiles[Random.Range(0, allgrassTiles.Length)];
    }




    void Start()
    {
        allowedGrassTiles = new List<TileBase>(allgrassTiles);
        grassTile = allgrassTiles[0];  // Initialize grassTile with the first tile in the grassTiles array
        forestTile = allforestTiles[0]; // Assuming the first forest tile is the default

        GenerateMap();
    }


    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Calculate the center offset
        int offsetX = width / 2;
        int offsetY = height / 2;

        // Step 1: Start with all grass tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = grassTile;  // Initially, set everything to grass
            }
        }

        // Step 4: Place forest clusters
        PlaceForestClusters();

        // Step 5: Remove any remaining grass tiles
        RemoveGrassTiles();

        // Step 6: Apply the final map to the Tilemap (centered)
        ApplyTilesToTilemap(offsetX, offsetY);
    }


    void RemoveGrassTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == grassTile)  // Check for any remaining grass tiles
                {
                    mapData[x, y] = null;  // Remove the grass tile by setting to null
                }
            }
        }
    }


    void ShuffleDirections()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            int randIndex = Random.Range(i, directions.Length);
            Vector2Int temp = directions[i];
            directions[i] = directions[randIndex];
            directions[randIndex] = temp;
        }
    }
    void PlaceForestClusters()
    {
        int numClusters = width / 5 * 2; // Controls total number of forest clusters
        int minClusterSize = 8;
        int maxClusterSize = 20;
        int maxAttempts = 100; // Prevent infinite loops

        for (int i = 0; i < numClusters; i++)
        {
            int attempts = 0;
            bool placed = false;

            while (!placed && attempts < maxAttempts)
            {
                int startX = Random.Range(2, width - 2);
                int startY = Random.Range(2, height - 2);

                // Check if it's too close to existing water
                if (HasNearbyForestCluster(startX, startY))
                {
                    attempts++;
                    continue;
                }

                int clusterSize = Random.Range(minClusterSize, maxClusterSize + 1);
                GenerateForestCluster(startX, startY, clusterSize);

                placed = true;
            }
        }
    }

    bool HasNearbyForestCluster(int x, int y)
    {
        int nearbyForestCount = 0;
        int minSeparation = 2; // Increase this to require more spacing

        for (int dx = -minSeparation; dx <= minSeparation; dx++)
        {
            for (int dy = -minSeparation; dy <= minSeparation; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip the tile itself

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (mapData[nx, ny] == forestTile)
                    {
                        nearbyForestCount++;
                        if (nearbyForestCount >= 10) return true; // Only break up forest clusters if too large
                    }
                }
            }
        }

        return false;
    }

    void GenerateForestCluster(int startX, int startY, int clusterSize)
    {
        Queue<Vector2Int> openList = new Queue<Vector2Int>();
        openList.Enqueue(new Vector2Int(startX, startY));

        int placedTiles = 0;

        while (openList.Count > 0 && placedTiles < clusterSize)
        {
            Vector2Int current = openList.Dequeue();

            if (mapData[current.x, current.y] == (allowedGrassTiles.Contains(mapData[current.x, current.y])))

            {
                mapData[current.x, current.y] = forestTile;
                placedTiles++;

                // Add random adjacent tiles to continue growing the cluster
                ShuffleDirections();

                foreach (Vector2Int dir in directions)
                {
                    Vector2Int next = new Vector2Int(current.x + dir.x, current.y + dir.y);

                    if (next.x > 0 && next.x < width - 1 && next.y > 0 && next.y < height - 1)
                    {
                        openList.Enqueue(next);
                    }
                }
            }
        }
    }

    void ApplyTilesToTilemap(int offsetX, int offsetY)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] != null)
                {
                    // Adjust tile position to center it
                    Vector3Int tilePosition = new Vector3Int(x - offsetX, y - offsetY, 0);

                    // Apply the tile to the Tilemap at the calculated position
                    tilemap.SetTile(tilePosition, mapData[x, y]);
                }
            }
        }
    }




    void SmoothTiles(TileBase targetTile, int clusterSize)
    {
        TileBase[,] newMapData = new TileBase[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y, targetTile);

                if (mapData[x, y] == targetTile)
                {
                    if (neighbors >= clusterSize) // Control cluster size
                        newMapData[x, y] = targetTile;
                    else if (neighbors == 0)
                        newMapData[x, y] = grassTile;
                    else
                        newMapData[x, y] = mapData[x, y];
                }
                else
                {
                    if (neighbors >= clusterSize - 1)
                        newMapData[x, y] = targetTile;
                    else
                        newMapData[x, y] = mapData[x, y];
                }
            }
        }
        mapData = newMapData;
    }


    int CountNeighbors(int x, int y, TileBase targetTile)
    {
        int count = 0;
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                if (mapData[nx, ny] == targetTile) count++;
            }
        }

        return count; //Ensure the function always returns a value
    }
}