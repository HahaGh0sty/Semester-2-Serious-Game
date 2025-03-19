using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
   private ResourceManager resourceManager;

    void Start()
    {
        // Find the ResourceManager in the scene
        resourceManager = FindObjectOfType<ResourceManager>();
      
      Level1();
    }

    

    void Level1()
    {
        InvokeRepeating("moneygain", 2f, 1f);
    }

  
     void moneygain()
    {
        if (resourceManager != null)
        {
            resourceManager.GildedBanana += 1;
            resourceManager.energy -= 1;
        }
    }
}
