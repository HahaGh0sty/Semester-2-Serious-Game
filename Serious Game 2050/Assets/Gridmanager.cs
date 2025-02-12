using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] GameObject GridParent;
    [SerializeField] private float TileStartX;
    [SerializeField] private float TileStartY;
    [SerializeField] private float TileEndX;
    [SerializeField] private float TileEndY;

    [SerializeField] public GameObject StartPosition;
    [SerializeField] public GameObject EndPosition;

    [SerializeField] private GameObject Tile;

    void Start()
    {
        GridParent = transform.gameObject;
        StartPosition = transform.GetChild(0).gameObject;
        EndPosition = transform.GetChild(1).gameObject;

        TileStartX = StartPosition.transform.position.x;
        TileStartY = StartPosition.transform.position.y;

        TileEndX = EndPosition.transform.position.x;
        TileEndY = EndPosition.transform.position.y;

        GenerateGrid();
       
    }

    private GameObject GetGridParent()
    {
        return GridParent;
    }

    void GenerateGrid()
    {
        for (float x = TileStartX; x < TileEndX; x++)
        {
            for (float y = TileStartY; y < TileEndY; y++)
            {
                GameObject spawnedGridspace = Instantiate(Tile, new Vector3(x, y), Quaternion.identity);
                spawnedGridspace.transform.SetParent(transform);
                spawnedGridspace.name = $"GridSpace {x} {y}";
            }
        }
    }
}

