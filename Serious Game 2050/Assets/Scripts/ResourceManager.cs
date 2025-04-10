using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IDataPersistence
{

    public int GildedBanana;
    public int wood;
    public int stone;
    public int energy;
    public int water;
    public int vervuiling;

   
    public void LoadData(GameData data)
    {
      this.GildedBanana = data.GildedBanana;
      this.energy = data.energy;
      this.wood = data.wood;
      this.stone = data.stone;
      this.water = data.water;
      this.vervuiling = data.vervuiling;
    }

    public void SaveData(ref GameData data)
    {
      data.GildedBanana = this.GildedBanana;
      data.energy = this.energy;
      data.wood = this.wood;
      data.stone = this.stone;
      data.water = this.water;
      data.vervuiling = this.vervuiling;
    }
 
}

