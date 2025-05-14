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
    private CreateBuildGhost ghostbuild;

    public int MoneyGet;
    public int EnergyGet;
    public int WoodGet;
    public int StoneGet;
    public int FishGet;
    public int OilGet;
    public int CrudeOilGet;
    public int GrainGet;
    public int CoalGet;
    public int PolutionGet;
    public int SteelGet;

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
            resourceManager.GildedBanana += this.MoneyGet;
            resourceManager.Energy += this.EnergyGet;
            resourceManager.Wood += this.WoodGet;
            resourceManager.Stone += this.StoneGet;
            resourceManager.Fish += this.FishGet;
            resourceManager.Coal += this.CoalGet;
            resourceManager.Oil += this.OilGet;
            resourceManager.CrudeOil += this.CrudeOilGet;
            resourceManager.Polution += this.PolutionGet;
            resourceManager.Steel += this.SteelGet;
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




}
