using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public GameObject activeTile;
    private TerrainManager _terrainManager;
    private ConstructionManager _constructionManager;
    private TerraformingManager _terraformingManager;
    private PopulationManager _populationManager;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _terrainManager = TerrainManager.instance;
        _constructionManager = ConstructionManager.instance;
        _terraformingManager = TerraformingManager.instance;
        _populationManager = PopulationManager.instance;
    }

    private void Update()
    {
        if (activeTile != null)
        {
            UseHexStats();
        }
    }

    private void UseHexStats()
    {
        //show tile stats
        OwnedTile activeTileComponent = activeTile.GetComponent<OwnedTile>();
        activeTileComponent.tileStatsUI.gameObject.SetActive(true);
        
        SetHexStats(activeTileComponent);
    }

    //TODO update
    public void Build(BuildingBlueprint buildingBlueprint)
    {
        _constructionManager.Build(buildingBlueprint, activeTile);
        SoundManager.PlaySound(SoundManager.Sound.build);
    }
    
    public void ChopTree()
    {
        _terraformingManager.ChopTree(activeTile);
    }

    public void SetHexAsActive(GameObject hex)
    {
        if (activeTile == null)
        {
            activeTile = hex;
        }
        else
        {
            activeTile.GetComponent<OwnedTile>().Unselect();
            activeTile = hex;
        }
        activeTile.GetComponent<OwnedTile>().tileStatsUI.gameObject.SetActive(true);
        UseHexStats();
    }

    private void SetHexStats(OwnedTile activeTileComponent)
    {
        Dictionary<string,TileStat> tileStatsUI = activeTileComponent.tileStatsUI.tileStatistics;
        foreach (KeyValuePair<string, TileStat> uielement in tileStatsUI)
        {
            activeTileComponent.UpdateTileStatisticsUI(uielement.Key);
        }
    }

    public void SetHexAsInActive()
    {
        activeTile.GetComponent<OwnedTile>().tileStatsUI.gameObject.SetActive(false);
        activeTile = null;
    }

    public void BuyHex(GameObject _borderingHex)
    {
        BorderingTile borderingTileComponent = _borderingHex.GetComponent<BorderingTile>();
        
        if (!IsHexObtainable(borderingTileComponent))
        {
            //TODO can't call buyHex on !isHexObtainable, optimize
            Debug.Log("Not enough humans!");
        }
        else
        {
            GameObject hex = _terrainManager.ConvertToOwnedHex(borderingTileComponent);
            _populationManager.CreateVillage(hex);
            
            var allAvailableHumans = _populationManager.AllAvailableHumans(borderingTileComponent.GetOwnedHexesAround());
            IEnumerable<Human> pickedHumans = allAvailableHumans.OrderBy(x => new Random().Next()).Take(borderingTileComponent.humanPrice);

            //move in to new hex / kill
            foreach (Human pickedHuman in pickedHumans)
            {
                pickedHuman.Die();
            }

            _terrainManager.CreateConcealedHexesAround(hex);
        }
    }

    public void RelocateHumanTo(OwnedTile tile, Village village, Human human)
    {
        _populationManager.SettleHumanInHex(tile, village, human);
    }

    public bool IsHexObtainable(BorderingTile tile)
    {
        List<OwnedTile> ownedHexesAround = tile.GetOwnedHexesAround();
        int hexPrice = tile.humanPrice;
        var allAvailableHumans = _populationManager.AllAvailableHumans(ownedHexesAround);
        if (allAvailableHumans.Count <= hexPrice)
        {
            return false;
        }
        return true;
    }

    public Biome GetActiveTileBiome()
    {
        return activeTile.GetComponent<OwnedTile>().biome;
    }
}
