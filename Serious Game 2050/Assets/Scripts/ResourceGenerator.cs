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

    [HideInInspector] public int[] resourceRequired = new int[(int)ResourceType.Count];
    [HideInInspector] public int[] resourceAddSubtract = new int[(int)ResourceType.Count];

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
        Debug.Log($"Placing " + building + " at cell {cellPos}");

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
                resourceManager.GildedBanana < resourceRequired[(int)ResourceType.Money] ||
                resourceManager.Energy < resourceRequired[(int)ResourceType.Energy] ||
                resourceManager.Wood < resourceRequired[(int)ResourceType.Wood] ||
                resourceManager.Stone < resourceRequired[(int)ResourceType.Stone] ||
                resourceManager.Fish < resourceRequired[(int)ResourceType.Fish] ||
                resourceManager.Coal < resourceRequired[(int)ResourceType.Coal] ||
                resourceManager.Oil < resourceRequired[(int)ResourceType.Oil] ||
                resourceManager.CrudeOil < resourceRequired[(int)ResourceType.CrudeOil] ||
                resourceManager.Steel < resourceRequired[(int)ResourceType.Steel] ||
                resourceManager.Polution < resourceRequired[(int)ResourceType.Polution]
               )
            {
                Debug.LogWarning("Every " + building + " doesn't have enough resources to function!");
                return;
            }

            resourceManager.GildedBanana += resourceAddSubtract[(int)ResourceType.Money];
            resourceManager.Energy += resourceAddSubtract[(int)ResourceType.Energy];
            resourceManager.Wood += resourceAddSubtract[(int)ResourceType.Wood];
            resourceManager.Stone += resourceAddSubtract[(int)ResourceType.Stone];
            resourceManager.Fish += resourceAddSubtract[(int)ResourceType.Fish];
            resourceManager.Coal += resourceAddSubtract[(int)ResourceType.Coal];
            resourceManager.Oil += resourceAddSubtract[(int)ResourceType.Oil];
            resourceManager.CrudeOil += resourceAddSubtract[(int)ResourceType.CrudeOil];
            resourceManager.Polution += resourceAddSubtract[(int)ResourceType.Polution];
            resourceManager.Steel += resourceAddSubtract[(int)ResourceType.Steel];
        }
    }
}