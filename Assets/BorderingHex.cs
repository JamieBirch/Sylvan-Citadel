using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BorderingHex : Hex
{
    public Vector3 hoverOffset;

    public Biome biome;
    public string description;
    
    // public Canvas features_canvas;
    public bool hasWater;
    public bool hasWood;
    public bool hasFood;

    public GameObject water;
    public GameObject wood;
    public GameObject food;
    
    public Canvas hexInfoCanvas;
    public Text descriptionText;
    public Text priceText;
    
    private int humanPrice;

    private void Start()
    {
        humanPrice = 0;
        description = createDescription();

        descriptionText.text = description;
        priceText.text = humanPrice.ToString();
        
        //ui
        /*if (hasWater)
        {
            water.SetActive(true);
        }
        if (hasWood)
        {
            wood.SetActive(true);
        }
        if (hasFood)
        {
            food.SetActive(true);
        }*/
        hexInfoCanvas.enabled = false;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        gameObject.transform.position += hoverOffset;
        hexInfoCanvas.enabled = true;
    }
    
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //explore bordering hex
        TerrainManager.instance.BuyHex(gameObject);
    }
    
    private void OnMouseExit()
    {
        gameObject.transform.position -= hoverOffset;
        hexInfoCanvas.enabled = false;
    }

    public string createDescription()
    {
        //TODO make Biome-specific
        string _features;
        if (!hasWater && !hasWood)
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
        }

        string _biome = "Grove";
        //TODO
        return _features + " " + _biome;
    }

    private int definePrice()
    {
        //TODO
        return 0;
    }
}
