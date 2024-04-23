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
    
    public string description;
    
    public bool hasUniqueResource;
    public bool hasSecondaryResource;
    public bool hasTertiaryResource;
    public bool hasRestriction;
    
    public Canvas hexInfoCanvas;
    public Text descriptionText;
    public Text priceText;
    // public Text holdTimerText;
    public Image holdTimerCircle;
    
    public int defaultPrice;
    public int humanPrice;
    
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
        humanPrice = definePrice();
        priceText.text = humansPricePreText + humanPrice;
        hexInfoCanvas.enabled = true;
    }
    
    public void OnMouseDrag()
    {
        if (!isObtainable)
        {
            PlayerMessageService.instance.ShowMessage("Not enough population in nearby tiles");
            Debug.Log("Not enough population in nearby tiles");
            return;
        }
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
        hexInfoCanvas.enabled = false;
    }

    public string createDescription()
    {
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
}
