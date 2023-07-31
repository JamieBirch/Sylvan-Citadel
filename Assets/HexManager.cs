using UnityEngine;

public class HexManager : MonoBehaviour
{
    public static HexManager instance;
    public GameObject activeHex;
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
    }

    public void SetHexAsInActive(GameObject hex)
    {
        activeHex = null;
    }
}
