using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFollowMouse : MonoBehaviour
{
    [SerializeField] public bool BuildingIsSelected;
    [SerializeField] public Camera Camera;
    [SerializeField] public bool SelectGridPlacement;
    [SerializeField] public bool NotPlacableHere;
    public GameObject Building;

    private GameObject ghostBuilding;

    void Start()
    {
        BuildingIsSelected = true;

        // Find the ghost building in the scene by tag
        ghostBuilding = GameObject.FindGameObjectWithTag("GhostBuilding");
    }

    void Update()
    {
        if (BuildingIsSelected && ghostBuilding != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Distance from the camera

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            ghostBuilding.transform.position = worldPosition;

 
          if (Input.GetMouseButtonDown(0) && NotPlacableHere == false)
        {
           Vector3 spawnPosition = ghostBuilding.transform.position;

           // Snap to grid (2D: only X and Y)
            float gridSize = 1f; // Adjust griyd size as needed
            Vector3 snappedPosition = new Vector3(
            Mathf.Round(spawnPosition.x / gridSize) * gridSize,
            Mathf.Round(spawnPosition.y / gridSize) * gridSize,
            spawnPosition.z // Keep the current Z to maintain render order
             );

            Instantiate(Building, snappedPosition, Quaternion.identity);
            Destroy(GameObject.FindWithTag("GhostBuilding"));
        }


            if (Input.GetMouseButtonDown(1))
            {
                Destroy(GameObject.FindWithTag("GhostBuilding"));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
    }

}
