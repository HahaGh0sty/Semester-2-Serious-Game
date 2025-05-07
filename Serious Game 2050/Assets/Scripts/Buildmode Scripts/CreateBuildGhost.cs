using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateBuildGhost : MonoBehaviour
{
    [Header("References")]
    public GameObject buildingGhostPrefab;
    public TileBase placedAnimatedTile;
    public Tilemap targetTilemap;

    private GameObject currentGhost;

    void Update()
    {
        HandleGhostFollowMouse();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceTile();
        }
    }

    public void SpawnGhost()
    {
        if (currentGhost != null) Destroy(currentGhost);

        currentGhost = Instantiate(buildingGhostPrefab);
        SetGhostAppearance(currentGhost, 0.5f); // semi-transparent
    }

    void HandleGhostFollowMouse()
    {
        if (currentGhost == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorld);
        Vector3 cellCenter = targetTilemap.GetCellCenterWorld(cellPos);

        currentGhost.transform.position = cellCenter;
    }

    void PlaceTile()
    {
        if (currentGhost == null)
        {
            Debug.Log("No ghost to place.");
            return;
        }

        if (placedAnimatedTile == null)
        {
            Debug.LogWarning("PlacedAnimatedTile is not assigned.");
            return;
        }

        if (targetTilemap == null)
        {
            Debug.LogWarning("TargetTilemap is not assigned.");
            return;
        }

        Vector3Int cellPos = targetTilemap.WorldToCell(currentGhost.transform.position);
        Debug.Log($"Placing tile at cell {cellPos}");

        targetTilemap.SetTile(cellPos, placedAnimatedTile);

        Destroy(currentGhost);
        currentGhost = null;
    }


    void SetGhostAppearance(GameObject ghost, float alpha)
    {
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
