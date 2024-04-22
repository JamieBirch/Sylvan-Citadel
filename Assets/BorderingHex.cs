using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BorderingHex : Hex
{
    public static TileManager TileManager;
    
    public Color availableColor;
    public Color nonAvailableColor;
    public Renderer rend;

    public bool isObtainable = false;
    
    // public Vector3 hoverOffset;

    // public Biome biome;
    public string description;
    
    public bool hasUniqueResource;
    [FormerlySerializedAs("hasPrimaryResource")] public bool hasSecondaryResource;
    [FormerlySerializedAs("hasSecondaryResource")] public bool hasTertiaryResource;
    public bool hasRestriction;
    
    // public bool hasWater;
    // public bool hasWood;

    public Canvas hexInfoCanvas;
    public Text descriptionText;
    public Text priceText;
    // public Text holdTimerText;
    public Image holdTimerCircle;
    
    public int defaultPrice;
    public int humanPrice;
    
    // public static float overlapRadius = HexUtils.HexSize;
    private float holdTimer;
    public float holdTimerDefault;

    public string humansPricePreText/* = "settlers required: "*/;
    // public string holdTimerPreText = "hold for: ";

    private void Start()
    {
        TileManager = TileManager.instance;
        
        humanPrice = definePrice();
        description = createDescription();

        descriptionText.text = description;
        priceText.text = humansPricePreText + humanPrice;
        
        hexInfoCanvas.enabled = false;
        holdTimer = holdTimerDefault;
        // holdTimerText.enabled = false;
        holdTimerCircle.enabled = false;
    }

    private void Update()
    {
        humanPrice = definePrice();
        if (TileManager.IsHexObtainable(this))
        {
            isObtainable = true;
            rend.material.color = availableColor;
        }
        else
        {
            isObtainable = false;
            rend.material.color = nonAvailableColor;
        }
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        // gameObject.transform.position += hoverOffset;
        humanPrice = definePrice();
        priceText.text = humansPricePreText + humanPrice;
        hexInfoCanvas.enabled = true;
        /*List<OwnedHex> ownedHexesAround = GetOwnedHexesAround();
        foreach (OwnedHex ownedHex in ownedHexesAround)
        {
            ownedHex.ShowSettlersAvailable();
        }*/
    }
    
    public void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!isObtainable)
        {
            PlayerMessageService.instance.ShowMessage("Not enough population in nearby tiles");
            Debug.Log("Not enough population in nearby tiles");
            return;
        }
        // holdTimerText.enabled = true;
        holdTimerCircle.enabled = true;
        holdTimer -= Time.deltaTime;
        // holdTimerText.text = holdTimerPreText + (int)holdTimer;
        holdTimerCircle.fillAmount = holdTimer / holdTimerDefault;
        if (holdTimer <= 0)
        {
            TileManager.BuyHex(gameObject);
        }
    }

    public void OnMouseUp()
    {
        holdTimer = holdTimerDefault;
        // holdTimerText.enabled = false;
        holdTimerCircle.enabled = false;
    }

    private void OnMouseExit()
    {
        // gameObject.transform.position -= hoverOffset;
        hexInfoCanvas.enabled = false;
        
        /*List<OwnedHex> ownedHexesAround = GetOwnedHexesAround();
        foreach (OwnedHex ownedHex in ownedHexesAround)
        {
            ownedHex.StopShowSettlersAvailable();
        }*/
    }

    public string createDescription()
    {
        //TODO make Biome-specific
        string _features;
        /*if (!hasWater && !hasWood)
        {
            _features = "Serene";
        } else if (hasWater && hasWood)
        {
            _features = "Bountiful";
        } else if (hasWater)
        {
            _features = "Lakeside";
        } else 
        {
            _features = "Fruitful";
        }*/

        string _biome;

        switch (biome)
        {
            case Biome.grove:
            {
                _biome = "Grove";
                break;
            }
            case Biome.forest:
            {
                _biome = "Forest";
                break;
            }
            case Biome.grassland:
            {
                _biome = "Grassland";
                break;
            }
            /*case Biome.river:
            {
                _biome = "River";
                break;
            }
            case Biome.swamp:
            {
                _biome = "Swamp";
                break;
            }
            case Biome.mountain:
            {
                _biome = "Mountains";
                break;
            }*/
            default:
            {
                Debug.Log("no tile for this biome");
                _biome = "???";
                break;
            }
        }
        //TODO
        return /*_features + " " + */_biome;
    }

    private int definePrice()
    {
        int price = defaultPrice;
        
        List<OwnedHex> ownedHexesAround = GetOwnedHexesAround();
        price -= ownedHexesAround.Count;

        if (price < 0)
        {
            return 0;
        }
        return price;
    }

    //debug
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }*/

    /*public List<OwnedHex> GetOwnedHexesAround()
    {
        List<OwnedHex> listOfAdjacentOwnedHexes = new List<OwnedHex>();
        Vector3 hexPosition = transform.position;

        var overlapColliders = Physics.OverlapSphere(hexPosition, overlapRadius);
        foreach (Collider _collider in overlapColliders)
        {
            if (_collider.TryGetComponent(out OwnedHex hexComponent))
            {
                listOfAdjacentOwnedHexes.Add(hexComponent);
                // Debug.Log("Got one!");
            }
        }

        return listOfAdjacentOwnedHexes;
    } */
}
