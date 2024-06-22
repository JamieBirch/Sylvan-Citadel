using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public GameObject activeTile;
    public int defaultTilePrice;
    public int currentTilePrice;
    
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
        currentTilePrice = defaultTilePrice;
        
        _terrainManager = TerrainManager.instance;
        _constructionManager = ConstructionManager.instance;
        _terraformingManager = TerraformingManager.instance;
        _populationManager = PopulationManager.instance;
    }

    private void Update()
    {
        /*if (activeTile != null)
        {
            UseTileStats(activeTile.GetComponent<OwnedTile>());
        }*/
    }

    private void UseTileStats(OwnedTile ownedTile)
    {
        //show tile stats
        ownedTile.tileStatsUI.gameObject.SetActive(true);
        
        SetHexStats(ownedTile);
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

    public void SetTileAsActive(GameObject hex)
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
        ShowTileStats(activeTile.GetComponent<OwnedTile>());
    }

    public void ShowTileStats(OwnedTile tile)
    {
        if (activeTile != null && tile.gameObject != activeTile)
        {
            HideTileStats(activeTile.GetComponent<OwnedTile>());
        }
        tile.tileStatsUI.gameObject.SetActive(true);
        UseTileStats(tile);
    }

    public void HideTileStats(OwnedTile tile)
    {
        if (activeTile != null && tile.gameObject != activeTile)
        {
            ShowTileStats(activeTile.GetComponent<OwnedTile>());
        }
        tile.tileStatsUI.gameObject.SetActive(false);
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
        HideTileStats(activeTile.GetComponent<OwnedTile>());
        activeTile = null;
    }

    public void BuyHex(GameObject _borderingTile)
    {
        BorderingTile borderingTileComponent = _borderingTile.GetComponent<BorderingTile>();
        
        if (!IsTileObtainable(borderingTileComponent))
        {
            //TODO can't call buyHex on !isHexObtainable, optimize
            Debug.Log("Not enough humans!");
        }
        else
        {
            GameObject tile = _terrainManager.ConvertToOwnedHex(borderingTileComponent);
            _populationManager.CreateVillage(tile);
            
            var allAvailableHumans = _populationManager.AllAvailableHumans(borderingTileComponent.GetOwnedHexesAround());
            IEnumerable<Human> pickedHumans = allAvailableHumans.OrderBy(x => new Random().Next()).Take(borderingTileComponent.humanPrice);

            //move in to new hex / kill
            foreach (Human pickedHuman in pickedHumans)
            {
                pickedHuman.Die();
            }

            _terrainManager.CreateConcealedTilesAround(tile);
        }
    }

    public void RelocateHumanTo(OwnedTile tile, Village village, Human human)
    {
        _populationManager.SettleHumanInTile(tile, village, human);
    }

    public bool IsTileObtainable(BorderingTile tile)
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

    public void ShowBuildAndTerraformPanels()
    {
        _constructionManager.ShowBuildPanel();
        _terraformingManager.ShowPanel();
    }

    public void HideBuildAndTerraformPanels()
    {
        _constructionManager.HideBuildPanel();
        _terraformingManager.HidePanel();
    }
}
