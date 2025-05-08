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
    }

    void Update()
    {
        UpdateUI();
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
        TryUpdate("energy", resources.energy);
        TryUpdate("wood", resources.wood);
        TryUpdate("stone", resources.stone);
        TryUpdate("vis", resources.vis);
        TryUpdate("olie", resources.Olie);
        TryUpdate("ruweolie", resources.RuweOlie);
        TryUpdate("graan", resources.Graan);
        TryUpdate("coal", resources.coal);
        TryUpdate("vervuiling", resources.vervuiling);
        TryUpdate("staal", resources.staal);
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
