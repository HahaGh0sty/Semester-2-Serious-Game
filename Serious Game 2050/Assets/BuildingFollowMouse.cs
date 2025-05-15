using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFollowMouse : MonoBehaviour
{
    [SerializeField] public bool NotPlacableHere;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            NotPlacableHere = !NotPlacableHere;
        }
    }

}
