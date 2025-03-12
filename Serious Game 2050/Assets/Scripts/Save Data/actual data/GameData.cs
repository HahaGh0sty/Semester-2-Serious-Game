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

    // Constructor - initializes values when a new game is started
    public GameData(int mapSeed)
    {
        this.GildedBanana = 0;
        this.wood = 0;
        this.stone = 0;
        this.energy = 0;
        this.water = 0;
        this.vervuiling = 0;
        this.mapseed = mapSeed; // Assign the generated map seed
    }
}
