using UnityEngine;
using UnityEngine.Serialization;

public abstract class BuildingBlueprint : MonoBehaviour
{
    public bool locked;
    public string name;
    public int woodPrice;
    public string description;
    public GameObject buildingPrefab;
    [FormerlySerializedAs("onlyOnePerTile")] public bool onlyBuildingOnTile;
    public bool blocksTerraforming;
    public bool blockTreeGrowth;

    public virtual bool IsBuildable()
    {
        GameObject activeTile = TileManager.instance.activeTile;
        if (activeTile != null)
        {
            OwnedTile tile = activeTile.GetComponent<OwnedTile>();
            if (!onlyBuildingOnTile)
            {
                return EnoughWood() && CanBuildOnTile(tile);
            }
            else
            {
                return NoBuildingsOnTile(tile) && 
                 EnoughWood() && CanBuildOnTile(tile);
            }
        }
        else
        {
            return false;
        }
    }

    private static bool NoBuildingsOnTile(OwnedTile tile)
    {
        return tile.buildings.Count == 0;
    }

    private static bool CanBuildOnTile(OwnedTile tile)
    {
        return tile.allowBuildingOnTile;
    }

    private bool EnoughWood()
    {
        return GameStats.GetWood() >= woodPrice;
    }

    public abstract bool IsShowable();
}
