using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnterGridAccess : MonoBehaviour
{
    [SerializeField] public Button GridAccessButton;
    [SerializeField] bool BuildModeOn = false;
    [SerializeField] List<GameObject> Grids = new List<GameObject>();
    private object unityGameObjects;
    public Button btn;

    // Start is called before the first frame update
    void Start()
    {
        GridAccessButton.onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed()
    {
        Debug.Log("Button Has been pressed");
        if (BuildModeOn == false)
        {
            BuildModeOn = true;
           GameObject g = GameObject.FindGameObjectWithTag("GridSpace");

        }
        else if (BuildModeOn == true)
        {
            BuildModeOn = false;
        }
    }
}
