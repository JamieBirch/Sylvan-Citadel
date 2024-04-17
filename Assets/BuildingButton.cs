using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Button button;
    public Building building;
    
    // Update is called once per frame
    void Update()
    {
        if (building.IsBuildable())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
