using UnityEngine;
using UnityEngine.UI;

public class TerrainButton : MonoBehaviour
{
    public Button button;
    
    // Update is called once per frame
    void Update()
    {
        if (CheckIfTreesAvailable())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
        
        
        
    }

    private bool CheckIfTreesAvailable()
    {
        if (TileManager.instance.activeTile == null)
        {
            return false;
        } else if (TileManager.instance.activeTile.GetComponent<OwnedHex>().GetWoodland() == null)
        {
            return false;
        } else if (TileManager.instance.activeTile.GetComponent<OwnedHex>().GetWoodland().getCount() > 0 )
        {
            return true;
        }

        return false;
    }

}
