using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
    public GameObject buttons;
    private ConstructionManager _constructionManager;
    private TerrainManager _terrainManager;
    private PopulationManager _populationManager;
    
    // public GameObject hexStats;
    // public GameObject tileStatPrefab;
    // public Text hexNameText;
    //TODO create formula
    public Text hexPopularityText;
    public Text hexPopulationText;
    public Text hexSettlersText;
    // public Text hexFruitsText;
    public Text hexBedsText;
    
    // private Random rnd = new Random();
    
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
        SetHexStats();
    }

    public void BuildHouse()
    {
        _constructionManager.BuildHouse(activeHex);
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

    private void SetHexStats()
    {
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        // Dictionary<string,TileStat> tileStatistics = activeHexComponent.tileStatsUI.tileStatistics;
        activeHexComponent.tileStatsUI.gameObject.SetActive(true);
        


        //remove?
        // hexNameText.text = activeHexComponent.Name;
        hexPopularityText.text = activeHexComponent.HexPopulation.ToString();
        hexPopulationText.text = activeHexComponent.HexPopulation.ToString();
        hexSettlersText.text = activeHexComponent.GetSettlersAvailable().ToString();
        // hexFruitsText.text = activeHexComponent.FruitsAvailable.ToString();
        hexBedsText.text = activeHexComponent.GetBedsAvailable().ToString();
        
        
        //TOFIX: now Instantiates prefabs every frame
        /*foreach (LandscapeFeature feature in activeHexComponent.LandscapeFeaturesDictionary.Values)
        {
            LandscapeFeatureType landscapeFeatureType = feature.getFeatureType();
            int count = feature.getCount();
            /*GameObject tileStatGO = Instantiate(tileStatPrefab, hexStats.transform);
            TileStat tileStat = tileStatGO.GetComponent<TileStat>();
            tileStat.SetName(landscapeFeatureType.ToString());
            tileStat.SetCount(count);#1#
        }*/
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
