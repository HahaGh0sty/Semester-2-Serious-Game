using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tilemap;

public class NewMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] allgrassTiles;
    public TileBase[] allwaterTiles;
    public TileBase[] allforestTiles;

    private TileBase grassTile;
    private TileBase waterTile;
    private TileBase forestTile;

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
    //public TileBase grasswaterdoubleNorthSE;
    //public TileBase grasswaterdoubleNorthSW;
    //public TileBase grasswaterdoubleSouthNE;
    //public TileBase grasswaterdoubleSouthNW;
    //public TileBase grasswaterdoubleEastNW;
    //public TileBase grasswaterdoubleEastSW;
    //public TileBase grasswaterdoubleWestNE;
    //public TileBase grasswaterdoubleWestSE;
    public TileBase grasswaterdoubleNESW;
    public TileBase grasswaterdoubleNWSE;
    //public TileBase grasswatercornertouchNETile;
    //public TileBase grasswatercornertouchNWTile;
    //public TileBase grasswatercornertouchSETile;
    //public TileBase grasswatercornertouchSWTile;


    public TileBase grasswaterisland;
    public TileBase airtile;


    public int width = 50;
    public int height = 50;
    public int minWaterClusterSize = 5;
    public int maxWaterClusterSize = 15;
    public int minForestClusterSize = 8;
    public int maxForestClusterSize = 20;

    private TileBase[,] mapData;
    private List<Vector2Int> waterPositions = new List<Vector2Int>();

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
        grassTile = allgrassTiles[0];
        waterTile = allwaterTiles[0];

        // Ensure lists are initialized
        allowedGrassTiles = new List<TileBase>(allgrassTiles);

        // Assign a default forest tile
        if (allforestTiles.Length > 0)
            forestTile = allforestTiles[0];

        GenerateMap(0);
    }


    void GenerateMap(int seed)
    {
        Random.InitState(System.Environment.TickCount); // Uses the current time as a seed
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = allgrassTiles[Random.Range(0, allgrassTiles.Length)];
            }
        }

        // Place water clusters
        PlaceWaterClusters();

        // Add Grass/Water borders
        PlaceGrassWaterBorders();

        // Place forest clusters
        PlaceForestClusters();

        // Apply to tilemap
        ApplyTilesToTilemap(width / 2, height / 2);
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

    //------------------------------------WATER------------------------------

    void PlaceWaterClusters()
    {
        int numClusters = width / 7 * 2; // Keep original cluster count
        int minClusterSize = 8; // Keep original cluster size
        int maxClusterSize = 18;

        List<Vector2Int> placedClusters = new List<Vector2Int>(); // Store cluster positions

        for (int i = 0; i < numClusters; i++)
        {
            int attempts = 20; // Max attempts to find a valid spot
            bool placed = false;

            while (attempts > 0 && !placed)
            {
                int startX = Random.Range(3, width - 3);
                int startY = Random.Range(3, height - 3);
                Vector2Int newClusterPos = new Vector2Int(startX, startY);

                // Ensure the cluster is at least 3 tiles away from others
                bool tooClose = false;
                foreach (Vector2Int existingCluster in placedClusters)
                {
                    if (Vector2Int.Distance(existingCluster, newClusterPos) < 5)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    GenerateWaterCluster(startX, startY, Random.Range(minClusterSize, maxClusterSize + 1));
                    placedClusters.Add(newClusterPos);
                    placed = true;
                }

                attempts--;
            }
        }
    }
    void RefreshTilemap()
    {
        tilemap.RefreshAllTiles(); // Force the Tilemap to update visuals
    }
    void GenerateWaterCluster(int startX, int startY, int clusterSize)
    {
        Queue<Vector2Int> openList = new Queue<Vector2Int>();
        openList.Enqueue(new Vector2Int(startX, startY));

        int placedTiles = 0;
        while (openList.Count > 0 && placedTiles < clusterSize)
        {
            Vector2Int current = openList.Dequeue();
            if (System.Array.Exists(allgrassTiles, tile => tile == mapData[current.x, current.y]))
            {
                mapData[current.x, current.y] = allwaterTiles[Random.Range(0, allwaterTiles.Length)];
                waterPositions.Add(current);
                placedTiles++;
            }

            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int next = current + dir;
                if (next.x >= 0 && next.x < width && next.y >= 0 && next.y < height)
                {
                    openList.Enqueue(next);
                }
            }
        }
        RefreshTilemap();
    }
    void PlaceGrassWaterBorders()
    {
        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,  // NESW
        new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) // Diagonals
        };

        HashSet<Vector2Int> checkedPositions = new HashSet<Vector2Int>();

        foreach (Vector2Int waterPos in waterPositions)
        {
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = waterPos + dir;

                if (!checkedPositions.Contains(neighborPos) && neighborPos.x >= 0 && neighborPos.x < width && neighborPos.y >= 0 && neighborPos.y < height)
                {
                    if (System.Array.Exists(allgrassTiles, tile => tile == mapData[neighborPos.x, neighborPos.y]))
                    {
                        mapData[neighborPos.x, neighborPos.y] = GetCorrectGrassWaterTile(neighborPos.x, neighborPos.y);
                        RefreshTilemap(); // Force update to ensure the tile appears
                    }
                    checkedPositions.Add(neighborPos);
                }
            }
        }
    }
    TileBase GetCorrectGrassWaterTile(int x, int y)
    {
        bool IsWaterLike(TileBase tile) => tile != null && tile == waterTile;

        bool hasWaterNorth = y + 1 < height && IsWaterLike(mapData[x, y + 1]);
        bool hasWaterEast = x + 1 < width && IsWaterLike(mapData[x + 1, y]);
        bool hasWaterSouth = y - 1 >= 0 && IsWaterLike(mapData[x, y - 1]);
        bool hasWaterWest = x - 1 >= 0 && IsWaterLike(mapData[x - 1, y]);

        bool hasWaterNE = x + 1 < width && y + 1 < height && IsWaterLike(mapData[x + 1, y + 1]);
        bool hasWaterNW = x - 1 >= 0 && y + 1 < height && IsWaterLike(mapData[x - 1, y + 1]);
        bool hasWaterSE = x + 1 < width && y - 1 >= 0 && IsWaterLike(mapData[x + 1, y - 1]);
        bool hasWaterSW = x - 1 >= 0 && y - 1 >= 0 && IsWaterLike(mapData[x - 1, y - 1]);

        int neswCount = (hasWaterNorth ? 1 : 0) + (hasWaterEast ? 1 : 0) + (hasWaterSouth ? 1 : 0) + (hasWaterWest ? 1 : 0);
        int diagonalCount = (hasWaterNE ? 1 : 0) + (hasWaterNW ? 1 : 0) + (hasWaterSE ? 1 : 0) + (hasWaterSW ? 1 : 0);

        if (neswCount >= 3)
        {
            return waterTile;
        }
        else
        {
            // **Step 1: GrassWaterIsland**
            if (neswCount == 4 && diagonalCount == 4)
            {
                return grasswaterisland;
            }

            // **Step 2: GrassWaterTouch**
            if (neswCount == 2)
            {
                if (hasWaterNorth && hasWaterEast) return grasswatertouchNETile;
                if (hasWaterNorth && hasWaterWest) return grasswatertouchNWTile;
                if (hasWaterSouth && hasWaterEast) return grasswatertouchSETile;
                if (hasWaterSouth && hasWaterWest) return grasswatertouchSWTile;
            }
            if (neswCount == 2 && diagonalCount == 2)
            {
                //if (hasWaterWest && hasWaterSouth && hasWaterNE && hasWaterSW) return grasswatercornertouchNETile;
                //if (hasWaterEast && hasWaterSouth && hasWaterNW && hasWaterSE) return grasswatercornertouchNWTile;
                //if (hasWaterWest && hasWaterNorth && hasWaterSE && hasWaterNW) return grasswatercornertouchSETile;
                //if (hasWaterEast && hasWaterNorth && hasWaterSW && hasWaterNE) return grasswatercornertouchSWTile;
            }

            // **Step 3: Edge Tiles**
            if (neswCount == 1)
            {
                if (hasWaterNorth) return grasswaternorthTile;
                if (hasWaterEast) return grasswatereastTile;
                if (hasWaterSouth) return grasswatersouthTile;
                if (hasWaterWest) return grasswaterwestTile;
            }

            if (neswCount == 1 && diagonalCount == 1)
            {
                //if (hasWaterNorth && hasWaterSW) return grasswaterdoubleNorthSW;
                //if (hasWaterNorth && hasWaterSE) return grasswaterdoubleNorthSE;
                //if (hasWaterSouth && hasWaterNW) return grasswaterdoubleSouthNW;
                //if (hasWaterSouth && hasWaterNE) return grasswaterdoubleSouthNE;
                //if (hasWaterEast && hasWaterNW) return grasswaterdoubleEastNW;
                //if (hasWaterEast && hasWaterSW) return grasswaterdoubleEastSW;
                //if (hasWaterWest && hasWaterNE) return grasswaterdoubleWestNE;
                //if (hasWaterWest && hasWaterSE) return grasswaterdoubleWestSE;
            }

            // **Step 4: Corner Tiles**
            if (diagonalCount == 1 && neswCount == 0)
            {
                TileBase cornerTile = null;
                if (hasWaterNE) cornerTile = grasswatercornerNETile;
                if (hasWaterNW) cornerTile = grasswatercornerNWTile;
                if (hasWaterSE) cornerTile = grasswatercornerSETile;
                if (hasWaterSW) cornerTile = grasswatercornerSWTile;

                if (cornerTile != null)
                {
                    return cornerTile;
                }
            }

            // **Step 5: Double Tile**
            if (diagonalCount == 2 && neswCount == 0)
            {
                if (hasWaterNE && hasWaterSW) return grasswaterdoubleNESW;
                if (hasWaterNW && hasWaterSE) return grasswaterdoubleNWSE;
            }
            return waterTile; // Default fallback instead of returning null
        }

    }

    //---------------------------------FOREST------------------------------------------

    void PlaceForestClusters()
    {
        int numClusters = width / 5 * 2; // Controls total number of forest clusters
        int minForestClusterSize = 8;
        int maxForestClusterSize = 20;
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

                int clusterSize = Random.Range(minForestClusterSize, maxForestClusterSize + 1);
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