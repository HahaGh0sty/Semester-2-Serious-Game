using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildGhost : MonoBehaviour
{
    [SerializeField] public GameObject BuildingType;
    [SerializeField] public GameObject ExistingGhostBuilding;
    [SerializeField] public bool GhostBuildingExists;

    public void SpawnObject()
    {
        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");

        if (ExistingGhostBuilding != null)
        {
            Destroy(ExistingGhostBuilding);
            Instantiate(BuildingType);
        }

        else if (ExistingGhostBuilding == null)
        {
            Instantiate(BuildingType);
        }
        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");
    }

    public void GhostBuildingExistsAlready()
    {
        if (GhostBuildingExists == false)
        {
            GhostBuildingExists = true;
        }
        else if (GhostBuildingExists == true)
        {
            GhostBuildingExists = false;
        }
    }
}