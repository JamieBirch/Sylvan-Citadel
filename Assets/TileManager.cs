using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public GameObject activeTile;
    public GameObject buttons;
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
        OwnedHex activeHexComponent = activeTile.GetComponent<OwnedHex>();
        activeHexComponent.tileStatsUI.gameObject.SetActive(true);
        
        SetHexStats(activeHexComponent);
    }

    public void Build(GameObject buildingPrefab)
    {
        _constructionManager.Build(buildingPrefab, activeTile);
    }
    
    /*public void BuildWell()
    {
        _constructionManager.BuildWell(activeTile);
    }
    
    public void BuildFoodStorage()
    {
        _constructionManager.BuildFoodStorage(activeTile);
    }*/
    
    public void ChopTree()
    {
        _terraformingManager.ChopTree(activeTile);
        // _terrainManager.ChopTree(activeHex);
    }

    public void SetHexAsActive(GameObject hex)
    {
        if (activeTile == null)
        {
            activeTile = hex;
        }
        else
        {
            activeTile.GetComponent<OwnedHex>().Unselect();
            activeTile = hex;
        }
        buttons.SetActive(true);
        activeTile.GetComponent<OwnedHex>().tileStatsUI.gameObject.SetActive(true);
        // hexStats.SetActive(true);
        UseHexStats();
    }

    private void SetHexStats(OwnedHex activeHexComponent)
    {
        Dictionary<string,TileStat> tileStatsUI = activeHexComponent.tileStatsUI.tileStatistics;
        foreach (KeyValuePair<string, TileStat> uielement in tileStatsUI)
        {
            activeHexComponent.UpdateTileStatisticsUI(uielement.Key);
        }
    }

    public void SetHexAsInActive()
    {
        activeTile.GetComponent<OwnedHex>().tileStatsUI.gameObject.SetActive(false);
        activeTile = null;
        buttons.SetActive(false);
        // hexStats.SetActive(false);
    }

    public void BuyHex(GameObject _borderingHex)
    {
        BorderingHex borderingHexComponent = _borderingHex.GetComponent<BorderingHex>();
        
        if (!IsHexObtainable(borderingHexComponent))
        {
            //TODO can't call buyHex on !isHexObtainable, optimize
            Debug.Log("Not enough humans!");
        }
        else
        {
            GameObject hex = _terrainManager.ConvertToOwnedHex(borderingHexComponent);
            _populationManager.CreateVillage(hex);
            
            // List<OwnedHex> ownedHexesAround = borderingHexComponent.GetOwnedHexesAround();
            // int hexPrice = borderingHexComponent.humanPrice;
            // var allAvailableHumans = _populationManager.AllAvailableHumans(ownedHexesAround);

            var allAvailableHumans = _populationManager.AllAvailableHumans(borderingHexComponent.GetOwnedHexesAround());
            // Random rnd = new Random();
            IEnumerable<Human> pickedHumans = allAvailableHumans.OrderBy(x => new Random().Next()).Take(borderingHexComponent.humanPrice);

            //move in to new hex / kill
            foreach (Human pickedHuman in pickedHumans)
            {
                // _populationManager.SettleHumanInHex(hex.GetComponent<OwnedHex>(), pickedHuman);
                pickedHuman.Die();
            }

            _terrainManager.CreateConcealedHexesAround(hex);
        }
    }

    public void RelocateHumanTo(OwnedHex hex, Village village, Human human)
    {
        // _populationManager.RelocateHuman(hex, human);
        _populationManager.SettleHumanInHex(hex, village, human);
    }

    public bool IsHexObtainable(BorderingHex hex)
    {
        List<OwnedHex> ownedHexesAround = hex.GetOwnedHexesAround();
        int hexPrice = hex.humanPrice;
        var allAvailableHumans = _populationManager.AllAvailableHumans(ownedHexesAround);
        if (allAvailableHumans.Count <= hexPrice)
        {
            return false;
        }
        return true;
    }
}
