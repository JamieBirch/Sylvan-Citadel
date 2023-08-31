using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = System.Random;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
    public GameObject buttons;
    private ConstructionManager _constructionManager;
    private TerrainManager _terrainManager;
    private PopulationManager _populationManager;
    
    public GameObject hexStats;
    public Text hexPopulationText;
    public Text hexFruitsText;
    public Text hexBedsText;
    
    private Random rnd = new Random();
    
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
            SetHexStats();
        }
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
        hexStats.SetActive(true);
        SetHexStats();
    }

    private void SetHexStats()
    {
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        hexPopulationText.text = activeHexComponent.HexPopulation.ToString();
        hexFruitsText.text = activeHexComponent.FruitsAvailable.ToString();
        hexBedsText.text = activeHexComponent.BedsAvailable.ToString();
    }

    public void SetHexAsInActive()
    {
        activeHex = null;
        buttons.SetActive(false);
        hexStats.SetActive(false);
    }

    public void BuyHex(GameObject _borderingHex)
    {
        BorderingHex borderingHexComponent = _borderingHex.GetComponent<BorderingHex>();
        
        /*List<OwnedHex> ownedHexesAround = borderingHexComponent.GetOwnedHexesAround();
        int hexPrice = borderingHexComponent.humanPrice;
        var allAvailableHumans = _populationManager.AllAvailableHumans(ownedHexesAround);*/
        if (!IsHexObtainable(borderingHexComponent))
        {
            //TODO can't call buyHex on !isHexObtainable, optimize
            Debug.Log("Not enough humans!");
        }
        else
        {
            GameObject hex = _terrainManager.ConvertToOwnedHex(borderingHexComponent);
            _populationManager.CreateVillage(hex);

            // var allAvailableHumans = _populationManager.AllAvailableHumans(borderingHexComponent.GetOwnedHexesAround());
            // IEnumerable<Human> pickedHumans = allAvailableHumans.OrderBy(x => rnd.Next()).Take(borderingHexComponent.humanPrice);

            //move in to new hex
            /*foreach (Human pickedHuman in pickedHumans)
            {
                _populationManager.SettleHumanInHex(hex.GetComponent<OwnedHex>(), pickedHuman);
            }*/

            _terrainManager.CreateConcealedHexesAround(hex);
        
            GameStats.OwnedHexes++;
        }
    }

    public void RelocateHumanTo(OwnedHex hex, Human human)
    {
        _populationManager.RelocateHuman(hex, human);
    }

    public bool IsHexObtainable(BorderingHex hex)
    {
        List<OwnedHex> ownedHexesAround = hex.GetOwnedHexesAround();
        int hexPrice = hex.humanPrice;
        var allAvailableHumans = _populationManager.AllAvailableHumans(ownedHexesAround);
        if (allAvailableHumans.Count < hexPrice)
        {
            return false;
        }
        return true;
    }
}
