using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] allgrassTiles;
    public TileBase[] allwaterTiles;

    private TileBase grassTile;
    private TileBase waterTile;

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
    public TileBase grasswaterdoubleNorthSE;
    public TileBase grasswaterdoubleNorthSW;
    public TileBase grasswaterdoubleSouthNE;
    public TileBase grasswaterdoubleSouthNW;
    public TileBase grasswaterdoubleEastNW;
    public TileBase grasswaterdoubleEastSW;
    public TileBase grasswaterdoubleWestNE;
    public TileBase grasswaterdoubleWestSE;
    public TileBase grasswaterdoubleNESW;
    public TileBase grasswaterdoubleNWSE;
    public TileBase grasswatercornertouchNETile;
    public TileBase grasswatercornertouchNWTile;
    public TileBase grasswatercornertouchSETile;
    public TileBase grasswatercornertouchSWTile;


    public TileBase grasswaterisland;
    public TileBase airtile;


    public int width = 50;
    public int height = 50;
    public int minWaterClusterSize = 5;
    public int maxWaterClusterSize = 15;

    private TileBase[,] mapData;
    private List<Vector2Int> waterPositions = new List<Vector2Int>();

    void Start()
    {
        grassTile = allgrassTiles[0];
        waterTile = allwaterTiles[0];
        GenerateMap(0);
    }

    void GenerateMap(int seed)
    {
        Random.InitState(System.Environment.TickCount); // Uses the current time as a seed
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Fill with grass first
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = grassTile;
            }
        }

        // Place water clusters
        PlaceWaterClusters();

        // Add Grass/Water borders
        PlaceGrassWaterBorders();

        // Apply to tilemap
        ApplyTilesToTilemap();
    }

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
            if (mapData[current.x, current.y] == grassTile)
            {
                mapData[current.x, current.y] = waterTile;
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
                    if (mapData[neighborPos.x, neighborPos.y] == grassTile)
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
                if (hasWaterWest && hasWaterSouth && hasWaterNE && hasWaterSW) return grasswatercornertouchNETile;
                if (hasWaterEast && hasWaterSouth && hasWaterNW && hasWaterSE) return grasswatercornertouchNWTile;
                if (hasWaterWest && hasWaterNorth && hasWaterSE && hasWaterNW) return grasswatercornertouchSETile;
                if (hasWaterEast && hasWaterNorth && hasWaterSW && hasWaterNE) return grasswatercornertouchSWTile;
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
                if (hasWaterNorth && hasWaterSW) return grasswaterdoubleNorthSW;
                if (hasWaterNorth && hasWaterSE) return grasswaterdoubleNorthSE;
                if (hasWaterSouth && hasWaterNW) return grasswaterdoubleSouthNW;
                if (hasWaterSouth && hasWaterNE) return grasswaterdoubleSouthNE;
                if (hasWaterEast && hasWaterNW) return grasswaterdoubleEastNW;
                if (hasWaterEast && hasWaterSW) return grasswaterdoubleEastSW;
                if (hasWaterWest && hasWaterNE) return grasswaterdoubleWestNE;
                if (hasWaterWest && hasWaterSE) return grasswaterdoubleWestSE;
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

    void ApplyTilesToTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] != null)
                {
                    tilemap.SetTile(new Vector3Int(x - width / 2, y - height / 2, 0), mapData[x, y]);
                }
            }
        }
    }
}
