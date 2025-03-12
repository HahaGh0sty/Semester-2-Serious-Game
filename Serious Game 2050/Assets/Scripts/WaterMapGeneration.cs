using System.Collections.Generic;
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
        GenerateMap();
    }

    void GenerateMap()
    {
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
        int numClusters = width / 5 * 2;
        for (int i = 0; i < numClusters; i++)
        {
            int startX = Random.Range(2, width - 2);
            int startY = Random.Range(2, height - 2);
            int clusterSize = Random.Range(minWaterClusterSize, maxWaterClusterSize + 1);
            GenerateWaterCluster(startX, startY, clusterSize);
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
    }

    void PlaceGrassWaterBorders()
    {
        foreach (Vector2Int waterPos in waterPositions)
        {
            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = waterPos + dir;
                if (neighborPos.x >= 0 && neighborPos.x < width && neighborPos.y >= 0 && neighborPos.y < height && mapData[neighborPos.x, neighborPos.y] == grassTile)
                {
                    mapData[neighborPos.x, neighborPos.y] = GetCorrectGrassWaterTile(neighborPos.x, neighborPos.y);
                }
            }
        }
    }

    TileBase GetCorrectGrassWaterTile(int x, int y)
    {
        bool IsWaterLike(TileBase tile)
        {
            if (tile == null) return false;
            return tile == waterTile;
        }

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
        return airtile;
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
