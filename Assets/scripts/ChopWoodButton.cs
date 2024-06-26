using UnityEngine;
using UnityEngine.UI;

public class ChopWoodButton : TerraformingButton
{
    // public Button button;
    
    // Update is called once per frame
    /*void Update()
    {
        if (CheckIfTreesAvailable())
        {
            // button.animator.Play("Normal");
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }*/

    public override bool IsDoable()
    {
        if (base.IsDoable())
        {
            return CheckIfTreesAvailable();
        }
        else
        {
            return false;
        }
    }


    private bool CheckIfTreesAvailable()
    {
        if (TileManager.instance.activeTile.GetComponent<OwnedTile>().GetWoodland() == null)
        {
            return false;
        } else if (TileManager.instance.activeTile.GetComponent<OwnedTile>().GetWoodland().getCount() > 0 )
        {
            return true;
        }

        return false;
    }

}
