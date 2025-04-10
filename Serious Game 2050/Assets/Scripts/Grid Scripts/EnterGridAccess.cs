using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnterGridAccess : MonoBehaviour
{
    [SerializeField] public Button GridAccessButton;
    [SerializeField] bool GridEnabled = false;
    [SerializeField] List<GameObject> Grids = new List<GameObject>();
    private object unityGameObjects;
    public Button btn;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GridAccessButton.GetComponent<Button>();
        btn.onClick.AddListener(GridAccess);
    }

    // Update is called once per frame
    private void OnMouseDown()
    {
    }

    void GridAccess()
    {
        Debug.Log("Button Has been pressed");
        if (GridEnabled == false)
        {
            GridEnabled = true;
           GameObject grid = GameObject.FindGameObjectWithTag("GridSpace");

        }
        else if (GridEnabled == true)
        {
            GridEnabled = false;
        }
    }
}
