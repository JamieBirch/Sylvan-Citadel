using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
    public GameObject buttons;
    private ConstructionManager _constructionManager;
    private TerrainManager _terrainManager;
    private PopulationManager _populationManager;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _constructionManager = ConstructionManager.instance;
        _terrainManager = TerrainManager.instance;
        _populationManager = PopulationManager.instance;
    }

    private void Update()
    {
        if (activeHex != null)
        {
            UseHexStats();
        }
    }

    private void UseHexStats()
    {
        //show tile stats
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        activeHexComponent.tileStatsUI.gameObject.SetActive(true);
        
        SetHexStats(activeHexComponent);
    }

    public void BuildHouse()
    {
        _constructionManager.BuildHouse(activeHex);
    }
    
    public void BuildWell()
    {
        _constructionManager.BuildWell(activeHex);
    }
    
    public void BuildFoodStorage()
    {
        _constructionManager.BuildFoodStorage(activeHex);
    }
    
    public void ChopTree()
    {
        _terrainManager.ChopTree(activeHex);
    }

    public void SetHexAsActive(GameObject hex)
    {
        if (activeHex == null)
        {
            activeHex = hex;
        }
        else
        {
            activeHex.GetComponent<OwnedHex>().Unselect();
            activeHex = hex;
        }
        buttons.SetActive(true);
        activeHex.GetComponent<OwnedHex>().tileStatsUI.gameObject.SetActive(true);
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
        activeHex.GetComponent<OwnedHex>().tileStatsUI.gameObject.SetActive(false);
        activeHex = null;
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
        
            GameStats.OwnedHexes++;
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
