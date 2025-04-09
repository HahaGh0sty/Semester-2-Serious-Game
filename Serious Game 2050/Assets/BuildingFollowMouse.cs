using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFollowMouse : MonoBehaviour
{
    [SerializeField] public bool BuildingIsSelected;
    [SerializeField] public Camera Camera;

    void Start()
    {
        BuildingIsSelected = true;
    }
    void Update()
    {
        if (BuildingIsSelected)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Distance from the camera

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
    }
}
