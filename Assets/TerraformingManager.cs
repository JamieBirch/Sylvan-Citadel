using UnityEngine;
using UnityEngine.UI;

public class TerraformingManager : MonoBehaviour
{
    public static TerraformingManager instance;
    
    public Button TerraformButton;
    public GameObject TerraformingButtons;
    
    public void ShowHidePanel()
    {
        if (TerraformingButtons.activeSelf)
        {
            TerraformingButtons.SetActive(false);
        }
        else
        {
            TerraformingButtons.SetActive(true);
        }
    }
    
    public void Update()
    {
        if (HexManager.instance.activeHex == null && TerraformButton.IsInteractable())
        {
            TerraformButton.interactable = false;
        }
        else if (HexManager.instance.activeHex != null && !TerraformButton.IsInteractable())
        {
            TerraformButton.interactable = true;
        }
    }
    
    public void ChopTree(GameObject activeHex)
    {
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        
        LandscapeFeatureWoodland landscapeFeatureWoodland = activeHexComponent.GetWoodland();
        
        GameObject biggestTree = landscapeFeatureWoodland.ChooseBiggestTree();
            
        if (biggestTree != null)
        {
            landscapeFeatureWoodland.ChopTree(biggestTree);
        }
        /*else
        {
            PlayerMessageService.instance.ShowMessage("No trees to chop! :(");
            Debug.Log("No trees to chop! :(");
        }*/
    }

}
