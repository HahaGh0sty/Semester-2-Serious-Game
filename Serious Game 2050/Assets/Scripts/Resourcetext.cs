using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resourcetext : MonoBehaviour
{
    private int moneycount = 0;

    private TextMeshProUGUI Resourcecounttext;
   
    private void awake()
    {
        Resourcecounttext = this.GetComponent<TextMeshProUGUI>();

    }

   

   
    private void update()
    {
        Resourcecounttext.text = "" + moneycount;
    }


    public void OnMoneyGet()
    {
        moneycount++;
    }
}
