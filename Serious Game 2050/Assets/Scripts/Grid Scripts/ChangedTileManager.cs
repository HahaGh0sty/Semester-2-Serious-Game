using System.Collections.Generic;
using UnityEngine;

public class NewGridManager : MonoBehaviour
{
    public GameObject[] Grids; // Normal Grid prefabs
    public GameObject CenterGrid; // The unique Grid that appears once
    public GameObject borderGrid; // Grid that surrounds the Center Grid
    public GameObject waterGrid; // Water Grid prefab
    public GameObject grassGrid; // Grass Grid prefab
    public GameObject separatingGrid; // "Grass/Water" separating Grid
    public int gridSize = 5; // Grid size (e.g., 5x5)\
    public Transform TilePosition; //position of the tile itself
    private float Gridsize;
    private Dictionary<Vector2Int, GameObject> placedGrids = new Dictionary<Vector2Int, GameObject>();
    private int maxWaterGrids;
    private int waterGridCount = 0;

    void Start()
    {
        TilePosition = gameObject.transform;
        if (Grids.Length == 0 || CenterGrid == null || borderGrid == null || waterGrid == null || grassGrid == null) return;

        Gridsize = GetGridsize(Grids[0]);
        maxWaterGrids = Mathf.FloorToInt((gridSize * gridSize) * 0.1f); // Limit water Grids to 10%
        GenerateGrid();
    }

    void GenerateGrid()
    {
        List<GameObject> GridList = new List<GameObject>(Grids);
        Shuffle(GridList); // Randomize the normal Grids

        Vector3 CenterGridPosition = TilePosition.transform.position;
        Vector2Int CenterGridGridPos = Vector2Int.zero;
        placedGrids[CenterGridGridPos] = Instantiate(CenterGrid, CenterGridPosition, Quaternion.identity);

        GenerateBordersAroundCenterGrid(CenterGridGridPos);

        float gridOffset = (gridSize / 2) * Gridsize;
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int position = new Vector2Int(x - gridSize / 2, y - gridSize / 2);
                if (position == CenterGridGridPos || placedGrids.ContainsKey(position)) continue;
                allPositions.Add(position);
            }
        }

        Shuffle(allPositions);

        foreach (Vector2Int position in allPositions)
        {
            if (placedGrids.ContainsKey(position)) continue;

            GameObject selectedGrid = GridList[Random.Range(0, GridList.Count)];

            Vector3 worldPosition = new Vector3(position.x * Gridsize, -position.y * Gridsize, 0);
            placedGrids[position] = Instantiate(selectedGrid, new Vector3(position.x, position.y), Quaternion.identity);
        }
    }

    void GenerateBordersAroundCenterGrid(Vector2Int CenterGridPos)
    {
        Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),
                                    new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)};

        foreach (Vector2Int dir in directions)
        {
            Vector2Int borderPos = CenterGridPos + dir;
            if (!placedGrids.ContainsKey(borderPos))
            {
                Vector3 worldPos = new Vector3(borderPos.x * Gridsize, -borderPos.y * Gridsize, 0);
                placedGrids[borderPos] = Instantiate(borderGrid, worldPos, Quaternion.identity);
            }
        }
    }


    void SetPosition(Vector3 CenterGridPosition)
    {
        TilePosition.transform.position = new Vector3(CenterGridPosition.x, CenterGridPosition.y, TilePosition.transform.position.z);
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

    float GetGridsize(GameObject Grid)
    {
        SpriteRenderer sr = Grid.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : 1.0f;
    }
}
