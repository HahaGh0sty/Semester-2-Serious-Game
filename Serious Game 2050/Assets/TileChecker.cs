using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChecker : MonoBehaviour
{
    public GameObject Grid;
    public bool tileMapGrassActive = false;
    public bool tileMapForestActive = false;
    public bool tileMapWaterActive = false;
    
    void Start()
    {
       if (tileMapForestActive || tileMapWaterActive || tileMapGrassActive == false)
        {
            
        }
    }

   void checkTile(Collision2D collision)
    {
        if (collision.gameObject.tag == "IsWaterTile")
        {
            tileMapWaterActive = true;
            Debug.Log("Water Tile under here!");
            return;
        }
        else if (collision.gameObject.tag == "IsForestTile")
        {
            tileMapForestActive = true;
            Debug.Log("Forest Tile under here!");
            return;
        }
        else if (collision.gameObject.tag == "IsGrassTile")
        {
            tileMapGrassActive = true;
            Debug.Log("Grass Tile under here!");
            return;

        }
}