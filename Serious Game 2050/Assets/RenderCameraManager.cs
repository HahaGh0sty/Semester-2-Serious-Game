using System.Collections.Generic;
using UnityEngine;

public class RenderCameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public int renderRadius = 15; // screen may only need 10, but we load 5 more for buffer
    public float tileSize = 1f;
    public int visibleRadius = 10;
    public int loadRadius = 14; // load more tiles before they appear


    private Vector2Int lastCameraTilePos;
    private HashSet<Vector2Int> loadedTiles = new HashSet<Vector2Int>();

    void Start()
    {
        UpdateVisibleTiles();
    }

    void LateUpdate()
    {
        Vector2Int currentCameraTile = WorldToTilePosition(mainCamera.transform.position);

        // Only update when camera crosses into a new tile
        if (currentCameraTile != lastCameraTilePos)
        {
            lastCameraTilePos = currentCameraTile;
            UpdateVisibleTiles();
        }
    }


    void UpdateVisibleTiles()
    {
        Vector2 cameraPos = mainCamera.transform.position;
        Vector2Int center = WorldToTilePosition(cameraPos);

        for (int x = -renderRadius; x <= renderRadius; x++)
        {
            for (int y = -renderRadius; y <= renderRadius; y++)
            {
                Vector2Int tilePos = new Vector2Int(center.x + x, center.y + y);
                if (!loadedTiles.Contains(tilePos))
                {
                    LoadTile(tilePos);
                    loadedTiles.Add(tilePos);
                }
            }
        }
    }

    Vector2Int WorldToTilePosition(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / tileSize), Mathf.FloorToInt(worldPos.y / tileSize));
    }

    void LoadTile(Vector2Int tilePos)
    {
        // Instantiate or enable tile at world position
        Vector2 worldPos = new Vector2(tilePos.x * tileSize, tilePos.y * tileSize);
        // Your code here to load/place the tile at worldPos
    }

    public void PreloadTilesAt(Vector3 worldPosition)
    {
        Vector2Int tilePos = WorldToTilePosition(worldPosition);
        if (tilePos != lastCameraTilePos)
        {
            lastCameraTilePos = tilePos;
        }
    }



}
