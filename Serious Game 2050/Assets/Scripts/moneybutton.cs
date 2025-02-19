using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneybutton : MonoBehaviour, IDataPersistence
{
    public int money;

    public void OnButtonPress()
    {
      this.money += 1;
    }

    public void LoadData(GameData data)
    {
      this.money = data.money;
    }

    public void SaveData(ref GameData data)
    {
      data.money = this.money;
    }
}
