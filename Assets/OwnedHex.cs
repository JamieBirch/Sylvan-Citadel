using System.Collections.Generic;
using UnityEngine;

public class OwnedHex : Hex
{
    private HexManager _hexManager;
    
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
        _hexManager = HexManager.instance;
        
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
            _hexManager.SetHexAsActive(gameObject);
        }
        else
        {
            Unselect();
        }
    }
    
    private void OnMouseExit()
    {
        if (!selected)
        {
            ColorToDefault();
        }
    }

    public void Unselect()
    {
        gameObject.transform.position -= selectOffset;
        selected = false;
        _hexManager.SetHexAsInActive(gameObject);
        ColorToDefault();
    }

    private void ColorToDefault()
    {
        rend.material.color = defaultColor;
    }
}
