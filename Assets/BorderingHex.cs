using UnityEngine;
using UnityEngine.EventSystems;

public class BorderingHex : Hex
{
    public Vector3 hoverOffset;

    public Biome biome;
    
    public Canvas canvas;
    public bool hasWater;
    public bool hasWood;
    public bool hasFood;

    public GameObject water;
    public GameObject wood;
    public GameObject food;

    private void Start()
    {
        //ui
        if (hasWater)
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
        }
        canvas.enabled = false;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        gameObject.transform.position += hoverOffset;
        canvas.enabled = true;
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
        canvas.enabled = false;
    }

    public string createDescription()
    {
        //TODO
        return "default description";
    }
}
