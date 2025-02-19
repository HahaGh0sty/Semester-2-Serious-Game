using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneratorTest : MonoBehaviour
{
    public GameObject Grids; //default grid
    public int GridSize = 32; //default grid 32x32 tile amount
    private float TileSize;
    [SerializeField] private Dictionary<Vector2Int, GameObject> PlacedTiles = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] Vector3 StartingGridPosition;
    [SerializeField] Vector3 StartPosition;





    private void Start()
    {
        TileSize = GetTileSize(Grids);
    }

    void GenerateGridMap()
    {
        Vector3 StartingGridPosition = Vector3.zero;
        Vector3 StartPosition = Vector3.zero;
        Vector2Int SpecialGridPos = Vector2Int.zero;
        PlacedTiles[SpecialGridPos] = Instantiate(Grids, StartingGridPosition, Quaternion.identity);

        float gridOffset = (GridSize / 2) * TileSize;
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Vector2Int position = new Vector2Int(x - GridSize / 2, y - GridSize / 2);
                if (position == SpecialGridPos || PlacedTiles.ContainsKey(position)) continue;
                allPositions.Add(position);
            }
        }
    }

    float GetTileSize(GameObject Grid)
    {
        SpriteRenderer sr = Grid.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : 1.0f;
    }
}
