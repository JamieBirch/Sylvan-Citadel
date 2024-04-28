using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Button button;
    public Building building;
    public GameObject tooltip;
    public Text buildingPrice;
    public Text buildingDescription;

    private void Start()
    {
        buildingPrice.text = "wood: " + building.woodPrice;
        buildingDescription.text = building.description;
    }

    // Update is called once per frame
    void Update()
    {
        if (building.IsBuildable())
        {
            button.animator.Play("Normal");
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void ShowTooltip()
    {
        tooltip.SetActive(true);
    }
    
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
