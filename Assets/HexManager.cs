using UnityEngine;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
    public GameObject buttons;
    private ConstructionManager _constructionManager;
    private TerrainManager _terrainManager;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _constructionManager = ConstructionManager.instance;
        _terrainManager = TerrainManager.instance;
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
    }

    public void SetHexAsInActive(GameObject hex)
    {
        activeHex = null;
        buttons.SetActive(false);
    }
}
