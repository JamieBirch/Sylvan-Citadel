using UnityEngine;

public class BorderingHex : Hex
{
    public Vector3 hoverOffset;
    public Canvas canvas;
    public bool hasWater;
    public bool hasWood;
    public bool hasFood;

    public GameObject water;
    public GameObject wood;
    public GameObject food;

    private void Start()
    {
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
        //explore bordering hex
        TerrainManager.instance.CreateOwnedHex(gameObject);
    }
    
    private void OnMouseExit()
    {
        gameObject.transform.position -= hoverOffset;
        canvas.enabled = false;
    }
    

}
