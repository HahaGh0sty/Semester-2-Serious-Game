using UnityEngine;

public class TileChecker : MonoBehaviour
{
    public GameObject Grid;
    public bool tileMapGrassActive = false;
    public bool tileMapForestActive = false;
    public bool tileMapWaterActive = false;

    void Start()
    {
        // Ensure at least one tile type is active
        if (!tileMapGrassActive && !tileMapForestActive && !tileMapWaterActive)
        {
            Debug.Log("No active tile maps detected at start.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CheckTile(collision.gameObject);
    }

    void CheckTile(GameObject obj)
    {
        if (obj.CompareTag("IsWaterTile"))
        {
            tileMapWaterActive = true;
            Debug.Log("Water Tile detected!");
        }
        else if (obj.CompareTag("IsForestTile"))
        {
            tileMapForestActive = true;
            Debug.Log("Forest Tile detected!");
        }
        else if (obj.CompareTag("IsGrassTile"))
        {
            tileMapGrassActive = true;
            Debug.Log("Grass Tile detected!");
        }
    }
}
