using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CreateBuildGhost : MonoBehaviour
{
    [Header("References")]
    public GameObject buildingGhostPrefab;

    public GameObject Building;
    public Tilemap targetTilemap;

    [SerializeField] public int MinimumGrassCount;
    [SerializeField] public int MinimumForestCount;
    [SerializeField] public int MinimumWaterCount;

    [SerializeField] public int MaximumGrassCount;
    [SerializeField] public int MaximumForestCount;
    [SerializeField] public int MaximumWaterCount;

    [SerializeField] public int BuildingCollisionCount;
    [SerializeField] public int GrassCollisionCount;
    [SerializeField] public int ForestCollisionCount;
    [SerializeField] public int WaterCollisionCount;

    [SerializeField] Color PlaceableHere;
    [SerializeField] Color NotPleacableHere;

    public bool IsOddNumberedSize;

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

    [SerializeField] public IsFucking IsFucking;

    private void Start()
    {
        ResourceManager = FindObjectOfType<ResourceManager>();
        PlaceableHere = new Color(0.5f, 1f, 0.4f, 0.9f);
        NotPleacableHere = new Color(1, 0.5f, 0.5f, 0.9f);
    }
    void Update()
    {

        HandleGhostFollowMouse();

        if (currentGhost != null)
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            // Checking all conditions, including water collision count
            bool hasEnoughCollisions =
            BuildingCollisionCount == 0 &&
            GrassCollisionCount >= MinimumGrassCount &&
            ForestCollisionCount >= MinimumForestCount &&
            WaterCollisionCount >= MinimumWaterCount;

            bool hasTooManyCollisions =
            MaximumGrassCount >= GrassCollisionCount &&
            MaximumForestCount >= ForestCollisionCount &&
            MaximumWaterCount >=  WaterCollisionCount;

            if (!hasEnoughCollisions || !hasTooManyCollisions)
            {
                Debug.LogWarning(currentGhost + " cannot be placed because not enough or too many collision points!" +
                    "  BuildingCount: " + BuildingCollisionCount + "!" +
                    "  GrassCount: " + GrassCollisionCount + "/ min " + MinimumGrassCount + "/max " + MaximumGrassCount +
                    "  ForestCount: " + ForestCollisionCount + "/min " + MinimumForestCount + "/max " + MaximumForestCount +
                    "  WaterCount: " + WaterCollisionCount + "/min " + MinimumWaterCount + "/max " + MaximumWaterCount);
                IsFucking.GhostBuildingSprite.material.color = NotPleacableHere;
                return;
            }

            IsFucking.GhostBuildingSprite.material.color = PlaceableHere;
            if (Input.GetMouseButtonDown(0))

                PlaceTile();
        }
    }

    public void SpawnGhost()
    {
        currentGhost = GameObject.FindGameObjectWithTag("GhostBuilding");

        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }

        currentGhost = Instantiate(buildingGhostPrefab);
        IsFucking = FindObjectOfType<IsFucking>();
        SetGhostAppearance(currentGhost, 0.5f); // semi-transparent
    }

    void HandleGhostFollowMouse()
    {
        if (currentGhost == null) return;

        BuildingCollisionCount = IsFucking.BuildingCollisionCount;
        GrassCollisionCount = IsFucking.GrassCollisionCount;
        ForestCollisionCount = IsFucking.ForestCollisionCount;
        WaterCollisionCount = IsFucking.WaterCollisionCount;

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
            //Debug.Log("No ghost to place.");
            return;
        }

        if (targetTilemap == null)
        {
            Debug.LogWarning("TargetTilemap is not assigned.");
            return;
        }
        if (
            ResourceManager.Wood < RequiredWood ||
            ResourceManager.Stone < RequiredStone ||
            ResourceManager.Energy < RequiredEnergy ||
            ResourceManager.GildedBanana < RequiredGildedBanana ||
            ResourceManager.CrudeOil < RequiredRawOil ||
            ResourceManager.Coal < RequiredCoal ||
            ResourceManager.Steel < RequiredSteel
            )
        {
            Debug.LogWarning("Not enough of a resource to build!");
            return;
        }

        //if (NotPlacableHere == false)
        //{
        //    Debug.Log("This building isn't placable here!");
        //    return;
        //}

        else
        {
            ResourceManager.Wood -= RequiredWood;
            ResourceManager.Stone -= RequiredStone;
            ResourceManager.Grain -= RequiredGrain;
            ResourceManager.Energy -= RequiredEnergy;
            ResourceManager.GildedBanana -= RequiredGildedBanana;
            ResourceManager.CrudeOil -= RequiredRawOil;
            ResourceManager.Fish -= RequiredFish;
            ResourceManager.Oil -= RequiredOil;
            ResourceManager.Steel -= RequiredSteel;


            Vector3Int cellPos = targetTilemap.WorldToCell(currentGhost.transform.position);

            Debug.Log($"Placing " + Building + " at cell {cellPos}");


            Instantiate(Building, cellPos, Quaternion.identity);

            Destroy(currentGhost);
            currentGhost = null;
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Building")
    //    {
    //        NotPlacableHere = !NotPlacableHere;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Building")
    //    {
    //        NotPlacableHere = !NotPlacableHere;
    //    }
    //}

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
