using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IDataPersistence
{

     public int wood;
    public int stone;
    public int Graan;
    public int energy;
    public int vervuiling;
    public int GildedBanana;
    public int RuweOlie;
    public int Olie;
    public int vis;
    public int coal;
    public int staal;

   
    public void LoadData(GameData data)
    {
      this.GildedBanana = data.GildedBanana;
      this.energy = data.energy;
      this.wood = data.wood;
      this.stone = data.stone;
      this.vis = data.vis;
      this.coal = data.coal;
      this.Olie = data.Olie;
      this.RuweOlie = data.RuweOlie;
      this.Graan = data.Graan;
      this.vervuiling = data.vervuiling;
      this.staal = data.staal;
    }

    public void SaveData(ref GameData data)
    {
      data.GildedBanana = this.GildedBanana;
      data.energy = this.energy;
      data.wood = this.wood;
      data.stone = this.stone;
      data.vis = this.vis;
      data.Olie = this.Olie;
      data.RuweOlie = this.RuweOlie;
      data.Graan = this.Graan;
      data.coal = this.coal;
      data.vervuiling = this.vervuiling;
      data.staal = this.staal;
    }
 
}

