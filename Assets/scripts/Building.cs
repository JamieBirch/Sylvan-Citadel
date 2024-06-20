using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public OwnedTile tile;
    public string name;
    private bool selected;

    public GameObject BuildingInfo;
    
    public Text buildingNameUI;
    public Text descriptionUI;

    public void Demolish()
    {
        SoundManager.PlaySound(SoundManager.Sound.chop);
        tile.RemoveBuildingFromTile(this);
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
        Debug.Log("building selected");
        highlight();
        BuildingInfo.SetActive(true);
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
        Debug.Log("building unselected");
        selected = false;
        ColorToDefault();
        BuildingInfo.SetActive(false);
    }
    
    private void highlight()
    {
        // rend.materials[1].color = selectColor;
    }

    private void ColorToDefault()
    {
        // rend.materials[1].color = defaultColor;
    }
}