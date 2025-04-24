using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFollowMouse : MonoBehaviour
{
    [SerializeField] public bool BuildingIsSelected;
    [SerializeField] public Camera Camera;
    [SerializeField] public bool SelectGridPlacement;
    public GameObject Building;

    private GameObject ghostBuilding;

    void Start()
    {
        BuildingIsSelected = true;

        // Find the ghost building in the scene by tag
        ghostBuilding = GameObject.FindGameObjectWithTag("GhostBuilding");

        if (ghostBuilding == null)
        {
            Debug.LogWarning("No GameObject with tag 'Ghostbuilding' found in the scene!");
        }
    }

    void Update()
    {
        if (BuildingIsSelected && ghostBuilding != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Distance from the camera

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            ghostBuilding.transform.position = worldPosition;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(Building, ghostBuilding.transform.position, Quaternion.identity);
                Destroy(GameObject.FindWithTag("GhostBuilding"));
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(GameObject.FindWithTag("GhostBuilding"));
            }
        }
    }
}
