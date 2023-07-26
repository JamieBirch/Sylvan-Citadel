using System.Collections.Generic;
using UnityEngine;

public class OwnedHex : Hex
{
    public Color hoverColor;
    private Color defaultColor;
    
    public Vector3 selectOffset;

    
    public Renderer rend;

    public GameObject waterway;
    public GameObject woodland;
    public GameObject village;
    public List<GameObject> buildings;
    private bool selected;

    private void Start()
    {
        defaultColor = rend.material.color;
        buildings = new List<GameObject>();
        selected = false;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        rend.material.color = hoverColor;
    }

    void OnMouseDown()
    {
        //TODO select this Hex
        if (!selected)
        {
            gameObject.transform.position += selectOffset;
            selected = true;
        }
        else
        {
            gameObject.transform.position -= selectOffset;
            selected = false;
        }
    }
    
    private void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }
}
