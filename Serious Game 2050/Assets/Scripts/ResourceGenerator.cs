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
    resourceManager = FindObjectOfType<ResourceManager>();
    StartCoroutine(DelayedOverlapCheck());
    Level1();
}

IEnumerator DelayedOverlapCheck()
{
    yield return new WaitForSeconds(0.1f); // Give a small delay

    if (IsOverlappingWithBuilding())
    {
        Debug.Log("Overlapping building found, destroying " + gameObject.name);
        Destroy(this.gameObject);
    }
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
    // Ensure the collider is available
    Collider thisCollider = GetComponent<Collider>();
    if (thisCollider == null)
    {
        Debug.LogWarning("No collider found on building " + gameObject.name);
        return false;
    }

    // Draw the overlap box in the scene view (for debugging)
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(thisCollider.bounds.center, thisCollider.bounds.size);

    // Check for any colliders in the overlap area
    Collider[] hits = Physics.OverlapBox(
        thisCollider.bounds.center,
        thisCollider.bounds.extents, // This represents half the size
        transform.rotation // Use the rotation to match the building's actual facing
    );

    foreach (Collider hit in hits)
    {
        // Ensure we ignore the current object and only check for other "Building" objects
        if (hit.gameObject != this.gameObject && hit.CompareTag("Building"))
        {
            Debug.Log($"Overlap detected with {hit.gameObject.name}");
            return true; // There was an overlap with another building
        }
    }

    return false;
}




}
