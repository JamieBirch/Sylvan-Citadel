using System.Collections.Generic;
using UnityEngine;

public class OwnedHex : Hex
{
    public Color hoverColor;
    private Color defaultColor;
    public Renderer rend;

    public GameObject waterway;
    public GameObject woodland;
    public GameObject village;
    public List<GameObject> buildings;

    private void Start()
    {
        defaultColor = rend.material.color;
        buildings = new List<GameObject>();
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        rend.material.color = hoverColor;
    }
    
    private void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }
}
