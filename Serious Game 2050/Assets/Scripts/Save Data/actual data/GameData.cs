using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int GildedBanana;
    public int wood;
    public int stone;
    public int energy;
    public int water;
    public int vervuiling;
    public int mapseed;
    public List<Vector3> buildingPositions;

    // Constructor - initializes values when a new game is started
    public GameData(int mapSeed)
    {
        this.GildedBanana = 0;
        this.wood = 0;
        this.stone = 0;
        this.energy = 0;
        this.water = 0;
        this.vervuiling = 0;
        this.mapseed = mapSeed;
        this.buildingPositions = new List<Vector3>(); // List to store building positions
    }

    // Method to save the building positions (called in DataPersistenceManager)
    public void SaveBuildingPositions(GameObject[] buildings)
    {
        buildingPositions.Clear(); // Clear existing positions before saving
        foreach (GameObject building in buildings)
        {
            if (building != null)
            {
                buildingPositions.Add(building.transform.position); // Add each building's position
            }
        }
    }

    // Method to load the building positions and update existing building positions
    public void LoadBuildingPositions()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building"); // Find all buildings with the tag "Building"
        
        for (int i = 0; i < buildingPositions.Count && i < buildings.Length; i++)
        {
            if (buildings[i] != null)
            {
                buildings[i].transform.position = buildingPositions[i]; // Update the position of the building
            }
            else
            {
                Debug.LogWarning("Building not found in scene, skipping position update.");
            }
        }
    }
}
