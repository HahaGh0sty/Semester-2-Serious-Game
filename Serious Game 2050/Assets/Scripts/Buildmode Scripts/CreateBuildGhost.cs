using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateBuildGhost : MonoBehaviour
{
    [Header("References")]
    public GameObject buildingGhostPrefab;
    public TileBase placedAnimatedTile;
    public Tilemap targetTilemap;

    private GameObject currentGhost;

    public ResourceManager ResourceManager;
    public int RequiredWood;
    public int RequiredStone;
    public int RequiredGrain;
    public int RequiredEnergy;
    public int RequiredPollution;
    public int RequiredGildedBanana;
    public int RequiredRawOil;
    public int RequiredOil;
    public int RequiredFish;
    public int RequiredCoal;
    public int RequiredSteel;

    private void Start()
    {
        ResourceManager = FindObjectOfType<ResourceManager>();
    }
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
        if (RequiredWood >= ResourceManager.wood && RequiredStone >= ResourceManager.stone && RequiredEnergy >= ResourceManager.energy && RequiredGildedBanana >= ResourceManager.GildedBanana && RequiredRawOil >= ResourceManager.RuweOlie && RequiredCoal >= ResourceManager.coal && RequiredSteel >= ResourceManager.staal)
        {
            Debug.LogWarning("Not enough of a resource to build!");
            return;
        }

        ResourceManager.wood -= RequiredWood;
        ResourceManager.stone -= RequiredStone;
        ResourceManager.Graan -= RequiredGrain;
        ResourceManager.energy -= RequiredEnergy;
        ResourceManager.GildedBanana -= RequiredGildedBanana;
        ResourceManager.RuweOlie -= RequiredRawOil;
        ResourceManager.vis -= RequiredFish;
        ResourceManager.Olie -= RequiredOil;
        ResourceManager.staal -= RequiredSteel;

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
