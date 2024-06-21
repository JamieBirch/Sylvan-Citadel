using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Button button;
    //sb BuildingBlueprint
    [FormerlySerializedAs("building")] public BuildingBlueprint buildingBlueprint;
    public GameObject tooltip;
    public Text buildingName;
    public Text buildingPrice;
    public Text buildingDescription;

    private void Start()
    {
        buildingName.text = buildingBlueprint.name;
        buildingPrice.text = "wood: " + buildingBlueprint.woodPrice;
        buildingDescription.text = buildingBlueprint.description;
    }

    // Update is called once per frame
    void Update()       
    {
        if (buildingBlueprint.IsBuildable())
        {
            // button.animator.Play("Normal");
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
