using System.Collections.Generic;
using UnityEngine;

public class BuildButtonsPanel : MonoBehaviour
{
    public List<BuildingButton> buildingButtons = new List<BuildingButton>();

    // Update is called once per frame
    void Update()
    {
        foreach (BuildingButton buildingButton in buildingButtons)
        {
            Building building = buildingButton.building;
            if (building.IsShowable())
            {
                buildingButton.gameObject.SetActive(true);
            }
            else
            {
                buildingButton.gameObject.SetActive(false);
            }
        }
    }
}
