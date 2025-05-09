using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildGhost : MonoBehaviour
{
    [SerializeField] public GameObject BuildingType;
    [SerializeField] public GameObject ExistingGhostBuilding;

    public void SpawnObject()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Distance from the camera

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");

        if (ExistingGhostBuilding != null)
        {
            if (ExistingGhostBuilding != BuildingType)
            {
                Destroy(ExistingGhostBuilding);
                Instantiate(BuildingType, worldPosition, Quaternion.identity);
                ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");
            }
            else
            {
                return;
            }
        }

        else if (ExistingGhostBuilding == null)
        {

            Instantiate(BuildingType, worldPosition, Quaternion.identity);
        }
        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");
    }
}