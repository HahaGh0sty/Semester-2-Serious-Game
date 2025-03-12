using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tilemap;

public class RandomTilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] allgrassTiles; // Array for grass tiles
    public TileBase[] allwaterTiles; // Array for water tiles
    public TileBase[] allforestTiles; // Array for forest tiles

    private TileBase grassTile; // Default grass tile
    private TileBase waterTile; // Default water tile
    private TileBase forestTile; // Default forest tile

    public TileBase grasswaternorthTile;
    public TileBase grasswatereastTile;
    public TileBase grasswatersouthTile;
    public TileBase grasswaterwestTile;
    public TileBase grasswatercornerNWTile;
    public TileBase grasswatercornerNETile;
    public TileBase grasswatercornerSWTile;
    public TileBase grasswatercornerSETile;
    public TileBase grasswatertouchNETile;
    public TileBase grasswatertouchNWTile;
    public TileBase grasswatertouchSETile;
    public TileBase grasswatertouchSWTile;
    public TileBase grasswaterisland;

    public int width = 200;
    public int height = 200;
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
        allowedWaterTiles = new List<TileBase>(allwaterTiles);
        grassTile = allgrassTiles[0];  // Initialize grassTile with the first tile in the grassTiles array
        waterTile = allwaterTiles[0];  // Assuming the first water tile is the default
        forestTile = allforestTiles[0]; // Assuming the first forest tile is the default

       
    }


    public void GenerateMap()
    {
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Step 1: Start with all grass tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = GetRandomTile();
            }
        }

        // Step 2: Place water clusters
        PlaceWaterClusters();

        // Step 3: Convert grass near water to Grass/Water tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == (allowedGrassTiles.Contains(mapData[x, y]) && IsNextToTile(x, y, waterTile)))
                {
                    mapData[x, y] = GetCorrectGrassWaterTile(x, y);
                }
            }
        }

        // Step 4: Place forest tiles
        PlaceForestClusters();

        // Step 5: Apply final tiles to tilemap
        ApplyTilesToTilemap();
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

void GenerateWaterCluster(int startX, int startY, int clusterSize)
{
    Queue<Vector2Int> openList = new Queue<Vector2Int>();
    openList.Enqueue(new Vector2Int(startX, startY));

    int placedTiles = 0;

    while (openList.Count > 0 && placedTiles < clusterSize)
    {
        Vector2Int current = openList.Dequeue();

        // Ensure this condition checks correctly if the tile is a grass tile
        if (allowedGrassTiles.Contains(mapData[current.x, current.y]))
        {
            mapData[current.x, current.y] = waterTile;
            placedTiles++;

            // Add random adjacent tiles to continue growing the cluster
            ShuffleDirections();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = new Vector2Int(current.x + dir.x, current.y + dir.y);

                if (next.x >= 0 && next.x < width && next.y >= 0 && next.y < height) // Fix bounds check
                {
                    openList.Enqueue(next);
                }
            }
        }
    }
}



    void EnsureWaterClusterSeparation()
    {
        List<Vector2Int> tilesToReplace = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == waterTile)
                {
                    // If water is too clustered, mark it for replacement
                    if (HasNearbyWaterCluster(x, y))
                    {
                        tilesToReplace.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        // Replace water tiles safely, but only if they exceed the limit
        foreach (Vector2Int tilePos in tilesToReplace)
        {
            mapData[tilePos.x, tilePos.y] = grassTile; // Replace with grassTile
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

    bool IsNextToTile(int x, int y, TileBase targetTile)
    {
        // Define all eight possible directions (including diagonals)
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),    // Right
        new Vector2Int(-1, 0),   // Left
        new Vector2Int(0, 1),    // Up
        new Vector2Int(0, -1),   // Down
        new Vector2Int(1, 1),    // Top-right (Diagonal)
        new Vector2Int(-1, 1),   // Top-left (Diagonal)
        new Vector2Int(1, -1),   // Bottom-right (Diagonal)
        new Vector2Int(-1, -1)   // Bottom-left (Diagonal)
        };

        // Check all directions
        foreach (var direction in directions)
        {
            int nx = x + direction.x;
            int ny = y + direction.y;

            // Make sure we are within bounds
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                if (mapData[nx, ny] == targetTile)
                    return true;
            }
        }

        return false; // Return false if no adjacent tile matches
    }




    TileBase GetCorrectGrassWaterTile(int x, int y)
    {

        // Check direct adjacent water-like tiles (NESW)
        bool hasWaterNorth = (y + 1 < height);
        bool hasWaterEast = (x + 1 < width);
        bool hasWaterSouth = (y - 1 >= 0);
        bool hasWaterWest = (x - 1 >= 0);

        // Check diagonal water-like tiles (NE, NW, SE, SW)
        bool hasWaterNE = (x + 1 < width && y + 1 < height);
        bool hasWaterNW = (x - 1 >= 0 && y + 1 < height);
        bool hasWaterSE = (x + 1 < width && y - 1 >= 0);
        bool hasWaterSW = (x - 1 >= 0 && y - 1 >= 0);

        int neswCount = (hasWaterNorth ? 1 : 0) + (hasWaterEast ? 1 : 0) + (hasWaterSouth ? 1 : 0) + (hasWaterWest ? 1 : 0);
        int diagonalCount = (hasWaterNE ? 1 : 0) + (hasWaterNW ? 1 : 0) + (hasWaterSE ? 1 : 0) + (hasWaterSW ? 1 : 0);

        // **Step 1: Place GrassWaterCorner (if touching exactly 1 diagonal water-like tile)**
        if (diagonalCount == 1 && neswCount == 0)
        {
            if (hasWaterNE) return grasswatercornerNETile;
            if (hasWaterNW) return grasswatercornerNWTile;
            if (hasWaterSE) return grasswatercornerSETile;
            if (hasWaterSW) return grasswatercornerSWTile;
        }

        // **Step 2: Place GrassWaterTouch (if touching exactly 2 NESW water-like tiles)**
        if (neswCount == 2)
        {
            if (hasWaterNorth && hasWaterEast) return grasswatertouchNETile;
            if (hasWaterNorth && hasWaterWest) return grasswatertouchNWTile;
            if (hasWaterSouth && hasWaterEast) return grasswatertouchSETile;
            if (hasWaterSouth && hasWaterWest) return grasswatertouchSWTile;
        }

        // **Step 3: Place Edge Tiles (if touching at least 1 NESW water-like tile)**
        if (neswCount >= 1)
        {
            if (hasWaterNorth) return grasswaternorthTile;
            if (hasWaterEast) return grasswatereastTile;
            if (hasWaterSouth) return grasswatersouthTile;
            if (hasWaterWest) return grasswaterwestTile;
        }

        // **Step 4: Place Island Tiles (if touching exactly 4 NESW water-like tiles)**
        if (diagonalCount >= 3 && neswCount >= 3)
        {
            return grasswaterisland;
        }

        // **Final Step: Default to Normal Grass Tile**
        return grassTile;
    }



    void EnsureGrassWaterTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == (allowedGrassTiles.Contains(mapData[x, y])))
                {
                    TileBase correctTile = GetCorrectGrassWaterTile(x, y);
                    if (correctTile != grassTile) // If a Grass/Water tile is needed, place it
                    {
                        mapData[x, y] = correctTile;
                    }
                }
            }
        }
    }

}