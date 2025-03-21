using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab; // Assign a tile prefab in Inspector
    public int gridWidth = 5;  // Grid width (X direction)
    public int gridHeight = 5; // Grid height (Y direction)

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 center = transform.position; // Start from the object's position
        int halfWidth = gridWidth / 2;
        int halfHeight = gridHeight / 2;

        for (int x = -halfWidth; x < halfWidth; x++)   
        {
            for (int y = -halfHeight; y < halfHeight; y++)
            {
                Vector3 spawnPosition = new Vector3(center.x + x, center.y + y, -5f);
                Instantiate(cellPrefab, spawnPosition, Quaternion.identity, transform);
                cellPrefab.name = (transform.parent.name + "'s Interactable Grid (" + x + " " + y + ")");
            }
        }
    }
}   