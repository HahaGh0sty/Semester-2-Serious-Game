using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelect : MonoBehaviour
{
    [SerializeField] private GameObject ThisGrid;
    [SerializeField] public bool Selected = false;
    [SerializeField] public bool Empty = false;

    [SerializeField] Color s_MouseOverColor;
    [SerializeField] Color s_OriginalColor;
    [SerializeField] SpriteRenderer s_Renderer;

    void Start()
    {

        ThisGrid = transform.gameObject;
        s_MouseOverColor = new Color(1, 0, 0, 0.5882353f);
        s_Renderer = GetComponent<SpriteRenderer>();
        s_OriginalColor = s_Renderer.material.color;
    }

    // Update is called once per frame
    private void OnMouseOver()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_MouseOverColor;   
        }
    }

    private void OnMouseExit()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_OriginalColor;
        }
    }

    private void OnMouseDown()
    {
        if (Selected == false)
        {
            Selected = true;
        }
        else if (Selected == true)
        {
            Selected = false;
        }
    }
}
