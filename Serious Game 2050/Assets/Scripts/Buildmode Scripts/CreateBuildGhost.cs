using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildGhost : MonoBehaviour
{
    [SerializeField] public GameObject BuildingType;
    [SerializeField] public GameObject ExistingGhostBuilding;

    public void SpawnObject()
    {
        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");

        if (ExistingGhostBuilding != null)
        {
            if (ExistingGhostBuilding != BuildingType)
            {
                Destroy(ExistingGhostBuilding);
                Instantiate(BuildingType);
                ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");
            }
            else
            {
                return;
            }
        }

        else if (ExistingGhostBuilding == null)
        {
            Instantiate(BuildingType);
        }
        ExistingGhostBuilding = GameObject.FindWithTag("GhostBuilding");
    }
}