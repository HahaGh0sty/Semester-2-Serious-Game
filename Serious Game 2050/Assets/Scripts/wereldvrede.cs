using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class wereldvrede : MonoBehaviour
{
    [System.Serializable]
    public class ResourceIcon
    {
        public string resourceName;
        public Sprite icon;
    }

    public ResourceManager resources;
    public GameObject resourceDisplayPrefab; // Assign in inspector
    public Transform displayParent; // The UI panel/parent where all displays go
    public List<ResourceIcon> icons; // Assign resource names and sprites in Inspector

    private Dictionary<string, ResourceDisplay> resourceDisplays = new Dictionary<string, ResourceDisplay>();

    void Start()
    {
        CreateUI();
        InvokeRepeating("UpdateUI", 0f, 0.1f);
    }

    void CreateUI()
    {
        foreach (var iconData in icons)
        {
            GameObject obj = Instantiate(resourceDisplayPrefab, displayParent);
            ResourceDisplay display = obj.GetComponent<ResourceDisplay>();
            resourceDisplays.Add(iconData.resourceName.ToLower(), display);
        }
    }

    void UpdateUI()
    {
        TryUpdate("gildedbanana", resources.GildedBanana);
        TryUpdate("energy", resources.Energy);
        TryUpdate("wood", resources.Wood);
        TryUpdate("stone", resources.Stone);
        TryUpdate("fish", resources.Fish);
        TryUpdate("oil", resources.Oil);
        TryUpdate("crude oil", resources.CrudeOil);
        TryUpdate("grain", resources.Grain);
        TryUpdate("coal", resources.Coal);
        TryUpdate("polution", resources.Polution);
        TryUpdate("steel", resources.Steel);
    }

    void TryUpdate(string key, int value)
    {
        if (resourceDisplays.ContainsKey(key))
        {
            Sprite icon = icons.Find(x => x.resourceName.ToLower() == key).icon;
            resourceDisplays[key].UpdateDisplay(icon, value);
        }
    }
}
