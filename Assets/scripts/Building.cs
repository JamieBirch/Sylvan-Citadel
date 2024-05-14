using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Building : MonoBehaviour
{
    public OwnedTile tile;
    private bool selected;

    public void Demolish()
    {
        tile.buildings.Remove(this);
        Destroy(gameObject);
    }
    
    public void OnMouseEnter()
    {
        highlight();
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
                Select();
            }
            else
            {
                Unselect();
            }
        }
    }
    
    private void Select()
    {
        selected = true;
        highlight();
        SoundManager.PlaySound(SoundManager.Sound.tile_select);
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
        selected = false;
        ColorToDefault();
    }
    
    private void highlight()
    {
        Debug.Log("building selected");
        // rend.materials[1].color = selectColor;
    }

    private void ColorToDefault()
    {
        Debug.Log("building unselected");
        // rend.materials[1].color = defaultColor;
    }
}