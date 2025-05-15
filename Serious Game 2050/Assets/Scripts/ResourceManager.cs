using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IDataPersistence
{

     public int Wood;
    public int Stone;
    public int Grain;
    public int Energy;
    public int Polution;
    public int GildedBanana;
    public int CrudeOil;
    public int Oil;
    public int Fish;
    public int Coal;
    public int Steel;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Wood += 500;
            Stone += 500;
            Grain += 500;   
            Energy += 500;
            Polution += 500;
            GildedBanana += 500;
            CrudeOil += 500;
            Oil += 500;
            Fish += 500;
            Coal += 500;
            Steel += 500;
        }
    }


    public void LoadData(GameData data)
    {
      this.GildedBanana = data.GildedBanana;
      this.Energy = data.energy;
      this.Wood = data.Wood;
      this.Stone = data.Stone;
      this.Fish = data.Fish;
      this.Coal = data.Coal;
      this.Oil = data.Oil;
      this.CrudeOil = data.CrudeOil;
      this.Grain = data.Grain;
      this.Polution = data.Polution;
      this.Steel = data.Steel;
    }

    public void SaveData(ref GameData data)
    {
      data.GildedBanana = this.GildedBanana;
      data.energy = this.Energy;
      data.Wood = this.Wood;
      data.Stone = this.Stone;
      data.Fish = this.Fish;
      data.Oil = this.Oil;
      data.CrudeOil = this.CrudeOil;
      data.Grain = this.Grain;
      data.Coal = this.Coal;
      data.Polution = this.Polution;
      data.Steel = this.Steel;
    }
 
}

