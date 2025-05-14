using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFucking : MonoBehaviour
{
    [SerializeField] public bool NotPlacableHere;
    [SerializeField] public int collisioncount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            collisioncount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            collisioncount--;
        }
    }
}
