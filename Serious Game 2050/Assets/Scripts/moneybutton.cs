using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneybutton : MonoBehaviour, IDataPersistence
{
    public int money;
    public int energy;

    void Start()
    {
      InvokeRepeating("moneygain", 2f, 1f);
    }

    public void moneygain()
    {
      this.money += 1;
      this.energy -= 1;
    }

    public void LoadData(GameData data)
    {
      this.money = data.money;
      this.energy = data.energy;
    }

    public void SaveData(ref GameData data)
    {
      data.money = this.money;
      data.energy = this.energy;
    }
 
}
