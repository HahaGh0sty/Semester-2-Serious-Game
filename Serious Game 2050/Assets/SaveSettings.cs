using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSettings
{
    public void SaveData()
    {
        //PlayerPrefs.SetFloat();
    }

    public void LoadData()
    {
        //PlayerPrefs.GetFloat();
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
