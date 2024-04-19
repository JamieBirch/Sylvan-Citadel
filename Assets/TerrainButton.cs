using UnityEngine;
using UnityEngine.UI;

public class TerrainButton : MonoBehaviour
{
    public Button button;
    
    // Update is called once per frame
    void Update()
    {
        if (TileManager.instance.activeTile == null && button.IsInteractable())
        {
            button.interactable = false;
        } else if (TileManager.instance.activeTile.GetComponent<OwnedHex>().GetWoodland() == null)
        {
            button.interactable = false;
        } else if (TileManager.instance.activeTile.GetComponent<OwnedHex>().GetWoodland().getCount() > 0 )
        {
            button.interactable = true;
        }
    }

}
