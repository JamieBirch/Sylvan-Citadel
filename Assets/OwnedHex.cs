using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OwnedHex : Hex
{
    private HexManager _hexManager;
    
    public Color hoverColor;
    private Color defaultColor;
    
    public Biome biome;
    
    public Vector3 selectOffset;

    public Renderer rend;

    public GameObject waterway;
    public GameObject woodland;
    public GameObject village;
    public List<GameObject> buildings;
    private bool selected;
    
    //Stats
    public int BedsAvailable;
    public int FruitsAvailable;
    public int HexPopulation;

    private void Start()
    {
        _hexManager = HexManager.instance;
        
        defaultColor = rend.material.color;
        buildings = new List<GameObject>();
        selected = false;

        BedsAvailable = 0;
        FruitsAvailable = 0;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        rend.material.color = hoverColor;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
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
        _hexManager.SetHexAsInActive();
        ColorToDefault();
    }

    private void ColorToDefault()
    {
        rend.material.color = defaultColor;
    }
}
