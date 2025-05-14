using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceManager resourceManager;
    public TileBase placedAnimatedTile;
    public Tilemap Tilemaptarget;
    private GameObject building;

    public enum ResourceType
    {
        Money,
        Energy,
        Wood,
        Stone,
        Fish,
        Oil,
        CrudeOil,
        Grain,
        Coal,
        Polution,
        Steel,
        Count
    }

    [HideInInspector] public int[] resourceLose = new int[(int)ResourceType.Count];
    [HideInInspector] public int[] resourceGet = new int[(int)ResourceType.Count];

    void Start()
    {
        if (Tilemaptarget == null)
        {
            Tilemaptarget = GameObject.Find("Test Tilemap").GetComponent<Tilemap>();
        }

        building = this.gameObject;
        resourceManager = FindObjectOfType<ResourceManager>();
        Level1();

        Vector3Int cellPos = Tilemaptarget.WorldToCell(building.transform.position);
        Debug.Log($"Placing tile at cell {cellPos}");

        Tilemaptarget.SetTile(cellPos, placedAnimatedTile);
    }

    void Level1()
    {
        InvokeRepeating("resourcegain", 2f, 1f);
    }

    void resourcegain()
    {
        if (resourceManager != null)
        {
            if (
                resourceManager.GildedBanana < resourceLose[(int)ResourceType.Money] ||
                resourceManager.Energy < resourceLose[(int)ResourceType.Energy] ||
                resourceManager.Wood < resourceLose[(int)ResourceType.Wood] ||
                resourceManager.Stone < resourceLose[(int)ResourceType.Stone] ||
                resourceManager.Fish < resourceLose[(int)ResourceType.Fish] ||
                resourceManager.Coal < resourceLose[(int)ResourceType.Coal] ||
                resourceManager.Oil < resourceLose[(int)ResourceType.Oil] ||
                resourceManager.CrudeOil < resourceLose[(int)ResourceType.CrudeOil] ||
                resourceManager.Steel < resourceLose[(int)ResourceType.Steel] ||
                resourceManager.Polution < resourceLose[(int)ResourceType.Polution]
               )
            {
                Debug.LogWarning(building + " doesn't have enough resources to function!");
                return;
            }

            resourceManager.GildedBanana += resourceGet[(int)ResourceType.Money];
            resourceManager.Energy += resourceGet[(int)ResourceType.Energy];
            resourceManager.Wood += resourceGet[(int)ResourceType.Wood];
            resourceManager.Stone += resourceGet[(int)ResourceType.Stone];
            resourceManager.Fish += resourceGet[(int)ResourceType.Fish];
            resourceManager.Coal += resourceGet[(int)ResourceType.Coal];
            resourceManager.Oil += resourceGet[(int)ResourceType.Oil];
            resourceManager.CrudeOil += resourceGet[(int)ResourceType.CrudeOil];
            resourceManager.Polution += resourceGet[(int)ResourceType.Polution];
            resourceManager.Steel += resourceGet[(int)ResourceType.Steel];
        }
    }
}

// New function: Check if another object with tag "Building" is on the same position
//private bool IsOverlappingWithBuilding()
//{
//    // Ensure the Collider2D is available
//    Collider2D thisCollider2D = GetComponent<Collider2D>();
//    if (thisCollider2D == null)
//    {
//        Debug.LogWarning("No Collider2D found on building " + gameObject.name);
//        return false;
//    }

//    // Draw the overlap box in the scene view (for debugging)
//    Gizmos.color = Color.red;
//    Gizmos.DrawWireCube(thisCollider2D.bounds.center, thisCollider2D.bounds.size);

//    // Check for any Collider2Ds in the overlap area
//    Collider2D[] hits = Physics.OverlapBox(
//        thisCollider2D.bounds.center,
//        thisCollider2D.bounds.extents, // This represents half the size
//        transform.rotation // Use the rotation to match the building's actual facing
//    );

//    foreach (Collider2D hit in hits)
//    {
//        // Ensure we ignore the current object and only check for other "Building" objects
//        if (hit.gameObject != this.gameObject && hit.CompareTag("Building"))
//        {
//            Debug.Log($"Overlap detected with {hit.gameObject.name}");
//            return true; // There was an overlap with another building
//        }
//    }

//    return false;
//}
