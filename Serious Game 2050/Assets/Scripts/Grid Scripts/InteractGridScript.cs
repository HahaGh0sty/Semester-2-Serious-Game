using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GridGenerator : MonoBehaviour
{
    private GameObject thisObject;
    public GameObject cellPrefab; // Assign a tile prefab in Inspector
    public int gridWidth = 5;  // Grid width (X direction)
    public int gridHeight = 5; // Grid height (Y direction)

    void Start()
    {
        thisObject = transform.gameObject;
        thisObject.SetActive(false);
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
                cellPrefab.name = (transform.parent.name + "'s Grid (" + x + " " + y + ")");
            }
        }
    }
    public void whenButtonClicked()
    {
        if (thisObject.activeInHierarchy == true)
        {
            thisObject.SetActive(false);

        }
        else
        {
            thisObject.SetActive(true);
        }
    }
}   