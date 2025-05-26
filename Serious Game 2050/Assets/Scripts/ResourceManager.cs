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
    private void Start()
    {
        Wood += 500;
        Stone += 200;
        Grain += 200;
        Energy += 200;
        Polution += 200;
        GildedBanana += 200;
        CrudeOil += 200;
        Oil += 200;
        Fish += 200;
        Coal += 200;
        Steel += 200;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Wood += 200;
            Stone += 200;
            Grain += 200;   
            Energy += 200;
            Polution += 200;
            GildedBanana += 200;
            CrudeOil += 200;
            Oil += 200;
            Fish += 200;
            Coal += 200;
            Steel += 200;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Wood = 0;
            Stone = 0;
            Grain = 0;
            Energy = 0;
            Polution = 0;
            GildedBanana =  0;
            CrudeOil = 0;
            Oil = 0;
            Fish = 0;
            Coal = 0;
            Steel = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GildedBanana += 200;
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

