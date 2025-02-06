using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private GameObject gridspace;


    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedGridspace = Instantiate(gridspace,new Vector3(x,y),Quaternion.identity);
                spawnedGridspace.name = $"GridSpace {x} {y}";
            }
        }
    }

}
