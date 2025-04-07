using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBuildModeScript : MonoBehaviour
{
    [SerializeField] private GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        thisObject = transform.gameObject;
        thisObject.SetActive(false);
    }
    public void whenButtonClicked()
    {
        if (thisObject.activeInHierarchy == true)
        {
            thisObject.SetActive(false);

        }
        else
        {
            thisObject.SetActive(true);
        }
    }
}
