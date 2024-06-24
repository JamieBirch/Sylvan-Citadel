using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BuildingBlueprint : MonoBehaviour
{
    public static ConstructionManager _constructionManager;
    
    public bool locked;
    public string name;
    [FormerlySerializedAs("woodPrice")] public int defaultWoodPrice;
    public int buildingWoodPrice;
    public string description;
    public GameObject buildingPrefab;
    [FormerlySerializedAs("onlyOnePerTile")] public bool onlyBuildingOnTile;
    public bool blocksTerraforming;
    public bool blockTreeGrowth;

    /*private void Start()
    {
        _constructionManager = ConstructionManager.instance;
    }*/

    private void Awake()
    {
        _constructionManager = ConstructionManager.instance;
    }

    /*private void Awake()
    {
        int buildPriceDiscount = _constructionManager.buildPriceDiscount;
        buildingWoodPrice = CalculateCurrentPrice(buildPriceDiscount);
    }*/

    public void CalculateCurrentPrice()
    {
        int buildPriceDiscount = _constructionManager.buildPriceDiscount;
        buildingWoodPrice = (int)(defaultWoodPrice * ((100f -  buildPriceDiscount) / 100f));
        // Debug.Log("calculating building price");
        // return defaultWoodPrice * ((100 - buildPriceDiscount) / 100);
    }

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
        return GameStats.GetWood() >= buildingWoodPrice;
    }

    public abstract bool IsShowable();
}
