using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFucking : MonoBehaviour
{
    [SerializeField] public bool NotPlacableHere;
    [SerializeField] public int BuildingCollisionCount = 0;
    [SerializeField] public int GrassCollisionCount = 0;
    [SerializeField] public int ForestCollisionCount = 0;
    [SerializeField] public int WaterCollisionCount = 0;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            BuildingCollisionCount++;
        }

        if (collision.gameObject.tag == "IsGrassTile")
        {
            GrassCollisionCount++;
        }

        if (collision.gameObject.tag == "IsForestTile")
        {
            ForestCollisionCount++;
        }

        if (collision.gameObject.tag == "IsWaterTile")
        {
            WaterCollisionCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            BuildingCollisionCount--;
        }

        if (collision.gameObject.tag == "IsGrassTile")
        {
            GrassCollisionCount--;
        }

        if (collision.gameObject.tag == "IsForestTile")
        {
            ForestCollisionCount--;
        }

        if (collision.gameObject.tag == "IsWaterTile")
        {
            WaterCollisionCount--;
        }
    }
}

