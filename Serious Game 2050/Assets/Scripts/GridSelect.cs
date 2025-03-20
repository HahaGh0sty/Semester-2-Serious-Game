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
    [SerializeField] Color s_MouseOverColor2;
    [SerializeField] SpriteRenderer s_Renderer;

    void Start()
    {

        ThisGrid = transform.gameObject;
        s_MouseOverColor = new Color(1, 0, 0, 0.5882353f);
        s_Renderer = GetComponent<SpriteRenderer>();
        s_OriginalColor = s_Renderer.material.color;
        s_MouseOverColor2 = new Color(1, 0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    private void OnMouseOver()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_MouseOverColor;   
        }

        if (Selected == true)
        {
            s_Renderer.material.color = s_MouseOverColor2;
        }
    }

    private void OnMouseExit()
    {
        if (Selected == false)
        {
            s_Renderer.material.color = s_OriginalColor;
        }

        if (Selected == true)
        {
            s_Renderer.material.color = s_MouseOverColor;
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
