using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase grassWaterTile;
    public TileBase forestTile; // NEW: Forest Tile

    public int width = 50;
    public int height = 50;
    public float initialWaterChance = 0.35f;
    public float initialForestChance = 0.25f; // NEW: Forest spawn chance
    public int smoothingIterations = 4;

    private TileBase[,] mapData;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();
        mapData = new TileBase[width, height];

        // Step 1: Randomly assign water, forest, or grass
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float roll = Random.value;
                if (roll < initialWaterChance)
                    mapData[x, y] = waterTile;
                else if (roll < initialWaterChance + initialForestChance)
                    mapData[x, y] = forestTile;
                else
                    mapData[x, y] = grassTile;
            }
        }

        // Step 2: Apply smoothing to both water and forest
        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothTiles(waterTile);
            SmoothTiles(forestTile);
        }

        // Step 3: Convert non-water tiles near water to Grass/Water
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == grassTile && IsNextToTile(x, y, waterTile))
                {
                    mapData[x, y] = grassWaterTile;
                }
            }
        }

        // Step 4: Apply map to Tilemap, centering at (0,0)
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

    void SmoothTiles(TileBase targetTile)
    {
        TileBase[,] newMapData = new TileBase[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y, targetTile);
                if (neighbors >= 3)
                    newMapData[x, y] = targetTile; // Strengthens clusters
                else if (neighbors <= 1)
                    newMapData[x, y] = grassTile; // Clears isolated tiles
                else
                    newMapData[x, y] = mapData[x, y]; // Keep existing tile
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
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                if (mapData[nx, ny] == targetTile) return true; // Return `true` if adjacent to `targetTile`
            }
        }
        return false; // Ensure a return in all cases
    }

}