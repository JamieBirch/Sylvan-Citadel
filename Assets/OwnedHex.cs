using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OwnedHex : Hex
{
    private HexManager _hexManager;
    
    public GameObject groveTile;
    public GameObject forestTile;
    public GameObject grasslandTile;
    public GameObject riverTile;
    public GameObject mountainTile;
    public GameObject swampTile;
    
    public Color hoverColor;
    private Color defaultColor;
    
    // public Biome biome;
    
    // public Vector3 selectOffset;

    public Renderer rend;

    public GameObject waterway;
    public GameObject woodland;
    public GameObject village;
    public List<House> houses;
    private bool selected;
    
    //Stats
    private int BedsAvailable;
    // public int FruitsAvailable;
    public int HexPopulation;

    public GameObject settlersAvailableCanvas;
    public Text settlersAvailableText;
    private int settlersAvailable;
    
    private void Start()
    {
        _hexManager = HexManager.instance;
        
        defaultColor = rend.material.color;
        houses = new List<House>();
        selected = false;

        BedsAvailable = 0;
        // FruitsAvailable = 0;
    }

    public void Update()
    {
        settlersAvailable = CalculateSettlersAvailable();
        
        if (houses.Count > 0)
        {
            BedsAvailable = CalcBedsAvailableSum();
            // Debug.Log("total beds: " + BedsAvailable);
        } 
    }

    public int GetBedsAvailable()
    {
        return BedsAvailable;
    }

    public void AddHouseToHex(House houseComponent)
    {
        houses.Add(houseComponent);
    }

    private int CalcBedsAvailableSum()
    {
        int BedsAvailableSum = 0;

        foreach (House house in houses)
        {
            BedsAvailableSum += house.GetBedsAvailable();
            // Debug.Log("adding beds: " + house.GetBedsAvailable() + "current sum: " + BedsAvailable);
        }
        return BedsAvailableSum;
    }

    private int CalculateSettlersAvailable()
    {
        return HexPopulation / 2;
    }

    public int GetSettlersAvailable()
    {
        return settlersAvailable;
    }

    public void SettleInHex(Human human)
    {
        // Debug.Log("settling " + human.Name + " in " + Name);
        if (human.homeHex != this)
        {
            // Debug.Log("new Hex!");
            _hexManager.RelocateHumanTo(this, village.GetComponent<Village>(), human);
        }
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
                // gameObject.transform.position += HexUtils.selectOffset;
                selected = true;
                _hexManager.SetHexAsActive(gameObject);
            }
            else
            {
                Unselect();
            }
        }
    }

    public void ShowSettlersAvailable()
    {
        settlersAvailableCanvas.SetActive(true);
        settlersAvailableText.text = settlersAvailable.ToString();
    }
    
    public void StopShowSettlersAvailable()
    {
        settlersAvailableCanvas.SetActive(false);
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
        // gameObject.transform.position -= HexUtils.selectOffset;
        selected = false;
        _hexManager.SetHexAsInActive();
        ColorToDefault();
    }

    private void ColorToDefault()
    {
        rend.material.color = defaultColor;
    }

    public bool NeighborsHaveAvailableBeds()
    {
        // Debug.Log("checking for available beds in nearby hexes");
        List<OwnedHex> ownedHexesAround = GetOwnedHexesAround();
        foreach (var ownedHex in ownedHexesAround)
        {
            if (ownedHex.BedsAvailable > 0)
            {
                // Debug.Log("there are available beds in nearby hexes");
                return true;
            } 
        }
        return false;
    }
}
