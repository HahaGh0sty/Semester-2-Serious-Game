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
        InvokeRepeating("UpdateUI", 0f, 2f);
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
        TryUpdate("Energy", resources.Energy);
        TryUpdate("wood", resources.Wood);
        TryUpdate("stone", resources.Stone);
        TryUpdate("vis", resources.Fish);
        TryUpdate("olie", resources.Oil);
        TryUpdate("ruweolie", resources.CrudeOil);
        TryUpdate("graan", resources.Grain);
        TryUpdate("coal", resources.Coal);
        TryUpdate("vervuiling", resources.Polution);
        TryUpdate("staal", resources.Steel);
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
