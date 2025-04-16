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
            resourceManager.vis += this.VisGet;
            resourceManager.coal += this.coalGet;
            resourceManager.Olie += this.OlieGet;
            resourceManager.RuweOlie += this.RuweOlieGet;
            resourceManager.vervuiling += this.VervuilingGet;
        }
    }
}
