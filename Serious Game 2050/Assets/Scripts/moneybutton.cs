using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneybutton : MonoBehaviour, IDataPersistence
{
    public int GildedBanana;
    public int energy;

    void Start()
    {
      InvokeRepeating("moneygain", 2f, 1f);
    }

    public void moneygain()
    {
      this.GildedBanana += 1;
      this.energy -= 1;
    }

    public void LoadData(GameData data)
    {
      this.GildedBanana = data.GildedBanana;
      this.energy = data.energy;
    }

    public void SaveData(ref GameData data)
    {
      data.GildedBanana = this.GildedBanana;
      data.energy = this.energy;
    }
 
}
