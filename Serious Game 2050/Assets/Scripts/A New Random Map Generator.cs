using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Something : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase[] grassTiles;
    public TileBase[] waterTiles;
    public TileBase[] forestTiles;
    public TileBase grasswatercornerNETile;
    public TileBase grasswatercornerNWTile;
    public TileBase grasswatercornerSETile;
    public TileBase grasswatercornerSWTile;
    public TileBase grasswatertouchNETile;
    public TileBase grasswatertouchNWTile;
    public TileBase grasswatertouchSETile;
    public TileBase grasswatertouchSWTile;

    public int width = 10;
    public int height = 10;

    private TileBase[,] mapData;

    private List<TileBase> allowedGrassTiles;
    private List<TileBase> allowedWaterTiles;
    private TileBase waterTile;
    private TileBase forestTile;
    Vector2Int[] directions =
{
    new Vector2Int(1, 0),   // Right
    new Vector2Int(-1, 0),  // Left
    new Vector2Int(0, 1),   // Up
    new Vector2Int(0, -1)   // Down
    };

    void Start()
    {
        allowedGrassTiles = new List<TileBase>(grassTiles);
        allowedWaterTiles = new List<TileBase>(waterTiles);
        waterTile = waterTiles[0]; // Assuming the first water tile is the default
        forestTile = forestTiles[0]; // Assuming the first forest tile is the default

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
                mapData[x, y] = grassTiles[Random.Range(0, grassTiles.Length)]; // Default to grass tile
            }
        }

        // Step 2: Place water clusters
        PlaceWaterClusters();

        // Step 3: Ensure water clusters have separation
        EnsureWaterClusterSeparation();

        // Step 4: Convert grass near water to Grass/Water tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == grassTiles[Random.Range(0, grassTiles.Length)] && IsNextToTile(x, y, waterTile))
                {
                    mapData[x, y] = GetCorrectGrassWaterTile(x, y);
                }
            }
        }

        // Step 5: Place forest tiles
        PlaceForestClusters();

        // Step 6: Apply final tiles to tilemap
        ApplyTilesToTilemap();
    }

    void EnsureWaterClusterSeparation()
    {
        // Ensure that water clusters are spaced apart by at least a certain distance
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == waterTile)
                {
                    // Check surrounding tiles and ensure separation
                    if (IsNextToTile(x, y, waterTile))
                    {
                        mapData[x, y] = grassTiles[Random.Range(0, grassTiles.Length)]; // Replace with grass if too close
                    }
                }
            }
        }
    }

    void ApplyTilesToTilemap()
    {
        tilemap.ClearAllTiles(); // Clears existing tiles

        int offsetX = width / 2;
        int offsetY = height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x - offsetX, y - offsetY, 0), mapData[x, y]);
            }
        }
    }

    void PlaceWaterClusters()
    {
        int numClusters = width / 5 * 2; // Controls total number of water clusters
        int minClusterSize = 5;
        int maxClusterSize = 15;
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
                if (HasNearbyWaterCluster(startX, startY))
                {
                    attempts++;
                    continue;
                }

                int clusterSize = Random.Range(minClusterSize, maxClusterSize + 1);
                GenerateWaterCluster(startX, startY, clusterSize);

                placed = true;
            }
        }
    }

    bool HasNearbyWaterCluster(int x, int y)
    {
        int minSeparation = 4;

        for (int dx = -minSeparation; dx <= minSeparation; dx++)
        {
            for (int dy = -minSeparation; dy <= minSeparation; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip current tile

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (mapData[nx, ny] == waterTile)
                    {
                        return true; // To close to watercluster
                    }
                }
            }
        }

        return false;
    }

    void GenerateWaterCluster(int startX, int startY, int clusterSize)
    {
        Queue<Vector2Int> openList = new Queue<Vector2Int>();
        openList.Enqueue(new Vector2Int(startX, startY));

        int placedTiles = 0;

        while (openList.Count > 0 && placedTiles < clusterSize)
        {
            Vector2Int current = openList.Dequeue();

            if (mapData[current.x, current.y] = grassTiles[Random.Range(0, grassTiles.Length)])
            {
                mapData[current.x, current.y] = waterTile;


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
        int numClusters = 3; // Define how many water clusters you want
        int maxClusterSize = 12; // Max size of a water cluster

        for (int i = 0; i < numClusters; i++)
        {
            int clusterX = Random.Range(1, width - 1);
            int clusterY = Random.Range(1, height - 1);

            int clusterSize = Random.Range(3, maxClusterSize);

            for (int j = 0; j < clusterSize; j++)
            {
                int xOffset = Random.Range(-1, 2); // Random offset for cluster spread
                int yOffset = Random.Range(-1, 2);

                int newX = Mathf.Clamp(clusterX + xOffset, 0, width - 1);
                int newY = Mathf.Clamp(clusterY + yOffset, 0, height - 1);

                mapData[newX, newY] = forestTile; // Place forest tile within the cluster
            }
        }
    }

    TileBase GetRandomTile()
    {
        // Return a random tile for grass or water
        if (Random.value > 0.5f)
        {
            return allowedGrassTiles[Random.Range(0, allowedGrassTiles.Count)];
        }
        else
        {
            return allowedWaterTiles[Random.Range(0, allowedWaterTiles.Count)];
        }
    }

    TileBase GetCorrectGrassWaterTile(int x, int y)
    {
        // Check surrounding tiles to determine if a Grass/Water corner or touch tile should be placed
        bool hasWaterNorth = IsNextToTile(x, y, waterTile, Direction.North);
        bool hasWaterSouth = IsNextToTile(x, y, waterTile, Direction.South);
        bool hasWaterEast = IsNextToTile(x, y, waterTile, Direction.East);
        bool hasWaterWest = IsNextToTile(x, y, waterTile, Direction.West);

        bool hasWaterNE = hasWaterNorth && hasWaterEast;
        bool hasWaterNW = hasWaterNorth && hasWaterWest;
        bool hasWaterSE = hasWaterSouth && hasWaterEast;
        bool hasWaterSW = hasWaterSouth && hasWaterWest;

        int diagonalCount = 0;
        int neswCount = 0;

        if (hasWaterNE) diagonalCount++;
        if (hasWaterNW) diagonalCount++;
        if (hasWaterSE) diagonalCount++;
        if (hasWaterSW) diagonalCount++;

        if (hasWaterNorth) neswCount++;
        if (hasWaterSouth) neswCount++;
        if (hasWaterEast) neswCount++;
        if (hasWaterWest) neswCount++;

        if (diagonalCount == 1 && neswCount == 0)
        {
            if (hasWaterNE) return grasswatercornerNETile;
            if (hasWaterNW) return grasswatercornerNWTile;
            if (hasWaterSE) return grasswatercornerSETile;
            if (hasWaterSW) return grasswatercornerSWTile;
        }

        if (neswCount == 2)
        {
            if (hasWaterNorth && hasWaterEast) return grasswatertouchNETile;
            if (hasWaterNorth && hasWaterWest) return grasswatertouchNWTile;
            if (hasWaterSouth && hasWaterEast) return grasswatertouchSETile;
            if (hasWaterSouth && hasWaterWest) return grasswatertouchSWTile;
        }

        return grassTiles[0]; // Default return in case no special tiles match
    }

    bool IsNextToTile(int x, int y, TileBase tile, Direction direction = Direction.All)
    {
        // Check adjacent tiles in the given direction (North, South, East, West)
        if (direction == Direction.All || direction == Direction.North)
        {
            if (y + 1 < height && mapData[x, y + 1] == tile)
                return true;
        }
        if (direction == Direction.All || direction == Direction.South)
        {
            if (y - 1 >= 0 && mapData[x, y - 1] == tile)
                return true;
        }
        if (direction == Direction.All || direction == Direction.East)
        {
            if (x + 1 < width && mapData[x + 1, y] == tile)
                return true;
        }
        if (direction == Direction.All || direction == Direction.West)
        {
            if (x - 1 >= 0 && mapData[x - 1, y] == tile)
                return true;
        }

        return false;
    }

    void DebugPrintTileCounts()
    {
        Dictionary<string, int> tileCount = new Dictionary<string, int>();

        foreach (var tile in mapData)
        {
            string tileName = tile != null ? tile.name : "null";
            if (!tileCount.ContainsKey(tileName))
            {
                tileCount[tileName] = 0;
            }
            tileCount[tileName]++;
        }

        foreach (var count in tileCount)
        {
            Debug.Log(count.Key + ": " + count.Value);
        }
    }

    public enum Direction
    {
        North,
        South,
        East,
        West,
        All
    }
}
