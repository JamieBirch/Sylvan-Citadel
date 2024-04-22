using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Button button;
    public Building building;
    public Text buildingPrice;

    private void Start()
    {
        buildingPrice.text = "wood: " + building.woodPrice;
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
}
