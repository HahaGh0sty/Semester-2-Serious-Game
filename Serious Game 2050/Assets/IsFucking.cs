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

    [SerializeField] Color BaseColor;
    [SerializeField] public SpriteRenderer GhostBuildingSprite;


    private void Start()
    {
        BaseColor = new Color(1, 1, 1, 1);
        GhostBuildingSprite = GetComponent<SpriteRenderer>();
        GhostBuildingSprite.color = BaseColor;
    }
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

        //note for later : Danny don't be a fucking dumbass and just make a seperage GameObject Under the current GhostBuilding with different Colliders so you can seperate them easily
        // - Danny
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

