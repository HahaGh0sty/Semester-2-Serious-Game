using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceManager resourceManager;

    public int MoneyGet;
    public int EnergyGet;
    public int WoodGet;
    public int StoneGet;
    public int VisGet;
    public int OlieGet;
    public int RuweOlieGet;
    public int GraanGet;
    public int coalGet;
    public int VervuilingGet;
    public int StaalGet;

    void Start()
    {
        // Check for overlap with a building
        if (IsOverlappingWithBuilding())
        {
            Destroy(gameObject); // Self-destruct if on same position as a building
            return;
        }

        // Find the ResourceManager in the scene
        resourceManager = FindObjectOfType<ResourceManager>();

        Level1();
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
            resourceManager.energy += this.EnergyGet;
            resourceManager.wood += this.WoodGet;
            resourceManager.stone += this.StoneGet;
            resourceManager.vis += this.VisGet;
            resourceManager.coal += this.coalGet;
            resourceManager.Olie += this.OlieGet;
            resourceManager.RuweOlie += this.RuweOlieGet;
            resourceManager.vervuiling += this.VervuilingGet;
            resourceManager.staal += this.StaalGet;
        }
    }

    // New function: Check if another object with tag "Building" is on the same position
   private bool IsOverlappingWithBuilding()
{
    GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
    float overlapThreshold = 0.9f; // how close is "same spot"

    foreach (GameObject building in buildings)
    {
        if (building != null && Vector3.Distance(building.transform.position, transform.position) < overlapThreshold)
        {
            return true;
        }
    }

    return false;
}

}
