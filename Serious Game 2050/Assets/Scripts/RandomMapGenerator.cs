using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase grassTile1;
    public TileBase grassTile2;
    public TileBase grassWaterTile;
    public TileBase grasswatercornerTile;
    public TileBase forestTile;
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


    public int width = 50;
    public int height = 50;
    public float initialWaterChance = 0.15f;
    public float initialForestChance = 0.2f;
    public int smoothingIterations = 1;
    public int waterClusterSize = 4;
    public int forestClusterSize = 4;



    private TileBase[,] mapData;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Step 1: Randomly assign water or grass
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.value < initialWaterChance)
                    mapData[x, y] = waterTile;
                else
                    mapData[x, y] = grassTile; // Default to grass
            }
        }

        // Step 2: Apply Water Smoothing
        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothTiles(waterTile, waterClusterSize);
        }

        // Step 3: Convert grass tiles near water into Grass/Water tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Check if the tile is grass and is next to water in any direction
                if (mapData[x, y] == grassTile && IsNextToTile(x, y, waterTile))
                {
                    // Here, we rotate the tile based on surrounding water tiles
                    mapData[x, y] = GetCorrectGrassWaterTile(x, y);
                }
            }
        }



        // Step 4: Randomly place forest tiles on remaining grass tiles (not Grass/Water)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == grassTile && Random.value < initialForestChance)
                {
                    mapData[x, y] = forestTile;
                }
            }
        }

        // Step 5: Apply Forest Smoothing
        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothTiles(forestTile, forestClusterSize);
        }

        // Step 6: Apply final tiles to tilemap
        ApplyTilesToTilemap();
    }




    void DebugPrintTileCounts()
    {
        int waterCount = 0, forestCount = 0, grassCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == waterTile) waterCount++;
                else if (mapData[x, y] == forestTile) forestCount++;
                else if (mapData[x, y] == grassTile) grassCount++;
            }
        }

        Debug.Log($"Initial Water: {waterCount}, Forest: {forestCount}, Grass: {grassCount}");
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
        // Check direct adjacent water tiles (NESW)
        bool hasWaterNorth = IsNextToTile(x, y + 1, waterTile);
        bool hasWaterEast = IsNextToTile(x + 1, y, waterTile);
        bool hasWaterSouth = IsNextToTile(x, y - 1, waterTile);
        bool hasWaterWest = IsNextToTile(x - 1, y, waterTile);

        // Check diagonal water tiles (NE, NW, SE, SW)
        bool hasWaterNE = IsNextToTile(x + 1, y + 1, waterTile);
        bool hasWaterNW = IsNextToTile(x - 1, y + 1, waterTile);
        bool hasWaterSE = IsNextToTile(x + 1, y - 1, waterTile);
        bool hasWaterSW = IsNextToTile(x - 1, y - 1, waterTile);

        // **Step 1: Place Corner Tiles (When touching exactly 2 water tiles in NESW)**
        if (hasWaterNorth && hasWaterEast && !hasWaterSouth && !hasWaterWest)
        {
            return grasswatercornerNETile;
        }
        else if (hasWaterNorth && hasWaterWest && !hasWaterEast && !hasWaterSouth)
        {
            return grasswatercornerNWTile;
        }
        else if (hasWaterSouth && hasWaterEast && !hasWaterNorth && !hasWaterWest)
        {
            return grasswatercornerSETile;
        }
        else if (hasWaterSouth && hasWaterWest && !hasWaterNorth && !hasWaterEast)
        {
            return grasswatercornerSWTile;
        }

        // **Step 2: Place Side Tiles (When touching exactly 1 water tile in NESW)**
        if (!hasWaterNorth)
        {
            return grasswaternorthTile;
        }
        if (!hasWaterEast)
        {
            return grasswatereastTile;
        }
        if (!hasWaterSouth)
        {
            return grasswatersouthTile;
        }
        if (!hasWaterWest)
        {
            return grasswaterwestTile;
        }


        // **Step 3: Place Corner Tiles (When touching exactly 2 water tiles in NESW)**
        if (!hasWaterNorth && !hasWaterEast)
        {
            return grasswatertouchNETile;
        }
        if (!hasWaterNorth && !hasWaterWest)
        {
            return grasswatertouchNWTile;
        }
        if (!hasWaterSouth && !hasWaterEast)
        {
            return grasswatertouchSETile;
        }
        if (!hasWaterSouth && !hasWaterWest)
        {
            return grasswatertouchSWTile;
        }

        // **Final Step: Default to Grass Tile if no water tiles around**
        return grassTile;
    }

}