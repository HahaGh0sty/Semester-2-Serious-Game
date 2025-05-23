using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBuildModeScript : MonoBehaviour
{
    [SerializeField] private GameObject thisObject;
    [SerializeField] private Button buildButton;
    // Start is called before the first frame update

    void Start()
    {
        thisObject = transform.gameObject;
        thisObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (buildButton != null)
            {
                buildButton.onClick.Invoke(); // Simulate the button click
            }
        }
    }

    public void whenBuildButtonClicked()
    {
        if (thisObject.activeInHierarchy == true)
        {
            thisObject.SetActive(false);
            Destroy(GameObject.FindWithTag("GhostBuilding"));

        }
        else
        {
            thisObject.SetActive(true);
        }
    }
}
 