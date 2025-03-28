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
   public int WaterGet;
   public int VervuilingGet;


    void Start()
    {
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
            resourceManager.water += this.WaterGet;
            resourceManager.vervuiling += this.VervuilingGet;
        }
    }
}
