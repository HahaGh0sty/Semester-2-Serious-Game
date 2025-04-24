using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBuildModeScript : MonoBehaviour
{
    [SerializeField] private GameObject thisObject;
    // Start is called before the first frame update
    public CreateBuildGhost GhostBuild;
    void Start()
    {
        thisObject = transform.gameObject;
        thisObject.SetActive(false);
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
