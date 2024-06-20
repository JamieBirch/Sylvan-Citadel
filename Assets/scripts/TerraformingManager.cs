using UnityEngine;
using UnityEngine.UI;

public class TerraformingManager : MonoBehaviour
{
    public static TerraformingManager instance;
    
    public Button TerraformButton;
    public GameObject TerraformingButtons;
    public GameObject TerraformingPanel;
    
    private void Awake()
    {
        instance = this;
    }
    
    /*public void ShowHidePanel()
    {
        if (TerraformingButtons.activeSelf)
        {
            TerraformingButtons.SetActive(false);
        }
        else
        {
            TerraformingButtons.SetActive(true);
        }
    }*/
    
    public void ShowPanel()
    {
        TerraformingPanel.SetActive(true);
    }
    
    public void HidePanel()
    {
        TerraformingPanel.SetActive(false);
    }
    
    /*public void Update()
    {
        if (TileManager.instance.activeTile == null && TerraformButton.IsInteractable())
        {
            TerraformButton.interactable = false;
        }
        else if (TileManager.instance.activeTile != null && !TerraformButton.IsInteractable())
        {
            TerraformButton.interactable = true;
        }
    }*/
    
    public void ChopTree(GameObject activeHex)
    {
        OwnedTile activeTileComponent = activeHex.GetComponent<OwnedTile>();
        
        LandscapeFeatureWoodland landscapeFeatureWoodland = activeTileComponent.GetWoodland();
        
        GameObject biggestTree = landscapeFeatureWoodland.ChooseBiggestTree();
            
        if (biggestTree != null)
        {
            landscapeFeatureWoodland.ChopTree(biggestTree);
        }

    }

}
