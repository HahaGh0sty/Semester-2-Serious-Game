using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateBuildGhost : MonoBehaviour
{
    [Header("References")]
    public GameObject buildingGhostPrefab;
   
    public GameObject Building;
    public Tilemap targetTilemap;

    private GameObject currentGhost;
    
    [SerializeField] public bool NotPlacableHere;

    void Update()
    {
        HandleGhostFollowMouse();

        if (Input.GetMouseButtonDown(0) && NotPlacableHere == false)
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

        Vector3 spawnPosition = currentGhost.transform.position;
        // Snap to grid (2D: only X and Y)
            float gridSize = 1f; // Adjust grid size as needed
            Vector3 snappedPosition = new Vector3(
            Mathf.Round(spawnPosition.x / gridSize) * gridSize,
            Mathf.Round(spawnPosition.y / gridSize) * gridSize,
            spawnPosition.z // Keep the current Z to maintain render order
             );

            Instantiate(Building, snappedPosition, Quaternion.identity);

        Destroy(currentGhost);
        currentGhost = null;


           
            
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
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
