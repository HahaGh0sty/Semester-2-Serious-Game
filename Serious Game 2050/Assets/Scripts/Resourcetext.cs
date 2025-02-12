using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resourcetext : MonoBehaviour, IDataPersistence
{
    private int money = 0;

    private TextMeshProUGUI ResourcecountText;
   
    private void awake()
    {
        ResourcecountText = this.GetComponent<TextMeshProUGUI>();

    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
    }

   

   
    private void update()
    {
        ResourcecountText.text = "" + money;
    }


    public void OnMoneyGet()
    {
        money++;
    }
}
