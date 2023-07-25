using UnityEngine;

public class OwnedHex : Hex
{
    public Color hoverColor;
    private Color defaultColor;
    
    private Renderer rend;
    
    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
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
