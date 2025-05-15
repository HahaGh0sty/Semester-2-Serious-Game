using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistence
{

    public int CurrentYear;
    public bool ArcadeMode;

    public float CurrentGameTime { get; internal set; }

    // Start is called before the first frame update
    public void newGame()
    {
        if (ArcadeMode == true)
        {
            CurrentYear = 2025;
        }
        //if (ArcadeMode = 0)
        //{
            //CurrentYear = 
       // }
    }
    void Start()
    {
        //replace the time with 720 for full game, right now the 15 is purely for testing
        InvokeRepeating("TimePass", 5f, 5f); 
    }

    void TimePass()
    {
        CurrentYear += 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if(CurrentYear = 2050)
       // {
            //gameover();
       // }
    }
    public void LoadData(GameData data)
    {
        this.CurrentYear = data.CurrentYear;
    }


    public void SaveData(ref GameData data)
    {
        data.CurrentYear = this.CurrentYear;
    }

    internal float GetTimePerDay()
    {
        throw new NotImplementedException();
    }
}
