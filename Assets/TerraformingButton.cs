using System;
using UnityEngine;
using UnityEngine.UI;

public class TerraformingButton : MonoBehaviour
{
    public Button button;

    private void Update()
    {
        if (IsDoable())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
    
    public virtual bool IsDoable()
    {
        GameObject activeTile = TileManager.instance.activeTile;
        if (activeTile != null)
        {
            OwnedTile tile = activeTile.GetComponent<OwnedTile>();
            if (tile.allowTileTerraforming)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public virtual bool IsShowable()
    {
        return true;
    }

}
