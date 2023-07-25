using UnityEngine;

public class BorderingHex : Hex
{
    public Vector3 hoverOffset;
    public Canvas canvas;

    private void Start()
    {
        canvas.enabled = false;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        gameObject.transform.position += hoverOffset;
        canvas.enabled = true;
    }
    
    private void OnMouseExit()
    {
        gameObject.transform.position -= hoverOffset;
        canvas.enabled = false;
    }
    

}
