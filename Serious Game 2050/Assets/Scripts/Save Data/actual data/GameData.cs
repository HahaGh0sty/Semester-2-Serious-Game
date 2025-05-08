using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public Vector3 position;
    public string prefabName; // The name of the prefab to instantiate

    public BuildingData(Vector3 pos, string prefab)
    {
        position = pos;
        prefabName = prefab;
    }
}

[System.Serializable]
public class GameData
{
    public int wood;
    public int stone;
    public int Graan;
    public int energy;
    public int vervuiling;
    public int GildedBanana;
    public int generatedValue;
    public int RuweOlie;
    public int Olie;
    public int vis;
    public int coal;
    public int staal;

    public List<BuildingData> savedBuildings; // Stores prefab name and position
    public Vector3 cameraPosition; // Camera position

    public GameData(int generatedValue)
    {
        this.wood = 0;
        this.stone = 0;
        this.Graan = 0;
        this.energy = 0;
        this.vervuiling = 0;
        this.GildedBanana = 0;
        this.generatedValue = 0;
        this.RuweOlie = 0;
        this.Olie = 0;
        this.vis = 0;
        this.coal = 0;
        this.staal = 0;
        this.savedBuildings = new List<BuildingData>();
        this.cameraPosition = Vector3.zero;
    }

    // Save buildings: assumes each building has a BuildingIdentifier component with prefab name
    public void SaveBuildingData(GameObject[] buildings)
    {
        savedBuildings.Clear();

        foreach (GameObject building in buildings)
        {
            if (building != null)
            {
                var identifier = building.GetComponent<BuildingIdentifier>();
                if (identifier != null)
                {
                    savedBuildings.Add(new BuildingData(building.transform.position, identifier.prefabName));
                }
                else
                {
                    Debug.LogWarning("Missing BuildingIdentifier on building.");
                }
            }
        }
    }

    // Load buildings: assumes a prefab dictionary is passed in with prefab names as keys
    public void LoadBuildingData(Dictionary<string, GameObject> prefabDict)
    {
        foreach (BuildingData data in savedBuildings)
        {
            if (prefabDict.TryGetValue(data.prefabName, out GameObject prefab))
            {
                GameObject.Instantiate(prefab, data.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab not found for: " + data.prefabName);
            }
        }
    }

    // Save the current camera position
    public void SaveCameraPosition(Camera camera)
    {
        if (camera != null)
        {
            cameraPosition = camera.transform.position;
        }
    }

    // Load the saved camera position
    public void LoadCameraPosition(Camera camera)
    {
        if (camera != null)
        {
            camera.transform.position = cameraPosition;
        }
    }
}
