using UnityEngine;
using UnityEngine.UI;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
    public GameObject buttons;
    private ConstructionManager _constructionManager;
    private TerrainManager _terrainManager;
    
    public GameObject hexStats;
    public Text hexPopulationText;
    public Text hexFruitsText;
    public Text hexBedsText;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _constructionManager = ConstructionManager.instance;
        _terrainManager = TerrainManager.instance;
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
}
