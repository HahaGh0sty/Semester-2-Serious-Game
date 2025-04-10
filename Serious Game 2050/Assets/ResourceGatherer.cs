using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] public GameObject thisObject;
    [SerializeField] public int GildedBanana;
    [SerializeField] public int energy;
    void Start()
    {
        thisObject = transform.gameObject;
        InvokeRepeating("moneygain", 2f, 1f);
    }


    void moneygain()
    {
        GildedBanana += 1;
        energy -= 1;
    }

    public void whenButtonClicked()
    {
        if(thisObject.activeInHierarchy == true)
        {
            thisObject.SetActive(false);
        }
        else
        {
            thisObject.SetActive(true);
        }
    }
}
