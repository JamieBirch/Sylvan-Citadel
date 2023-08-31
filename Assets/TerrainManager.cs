using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    public int StartTrees;
    
    public GameObject borderingHex;
    public GameObject farHex;
    public GameObject ownedHex;

    public GameObject woodland;
    public GameObject waterway;
    public GameObject fruitTree;
    public GameObject pineTree;
    public GameObject lake;

    public float overlapRadius = 1;
    
    public Vector3 firstTileCenter = Vector3.zero;
    public static float HexRadius = 3f;


    private Biome startBiome = Biome.grove;
    
    private void Awake()
    {
        instance = this;
    }

    public GameObject CreateStartHex()
    {
        GameObject startHex = CreateOwnedHex(startBiome, firstTileCenter);
        SpawnTrees(startHex, StartTrees);
        SpawnLakes(startHex, Utils.GenerateRandomIntMax(5));
        return startHex;
    }

    private GameObject CreateOwnedHex(Biome biome, Vector3 hexCenter)
    {
        GameObject newOwnedHex = Instantiate(ownedHex, hexCenter, Quaternion.identity, gameObject.transform);
        string districtName = NameGenerator.CreateDistrictName();
        newOwnedHex.name = districtName;
        OwnedHex hexComponent = newOwnedHex.GetComponent<OwnedHex>();
        hexComponent.Name = districtName;
        hexComponent.biome = biome;
        return newOwnedHex;
    }

    private void SpawnLakes(GameObject hex, int lakesNumber)
    {
        GameObject _waterway = Instantiate(waterway, hex.transform.position, Quaternion.identity, hex.transform);
        _waterway.name = "waterway";
        hex.GetComponent<OwnedHex>().waterway = _waterway;
        
        for (int i = 0; i < lakesNumber; i++)
        {
            SpawnLake(_waterway);
        }
    }

    private void SpawnLake(GameObject _waterway)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(_waterway.transform.position)/* + new Vector3(0, 0.875f, 0)*/;
        float lakeRotation = Utils.GenerateRandom(0, 360f);
        GameObject newLake = Instantiate(lake, position, Quaternion.AngleAxis(lakeRotation, Vector3.up), _waterway.transform);
        float randomScale = Utils.GenerateRandom(0.3f, 1.5f);
        newLake.transform.localScale = new Vector3(randomScale, 1, randomScale);
        
        _waterway.GetComponent<Waterway>().lakes.Add(newLake);
    }

    public void SpawnTrees(GameObject hex, int treesNumber)
    {
        // GameObject _woodland = hex.GetComponent<OwnedHex>().woodland;
        // if (woodland == null)
        // {
            GameObject _woodland = Instantiate(woodland, hex.transform.position, Quaternion.identity, hex.transform);
            _woodland.name = "woodland";

            OwnedHex hexComponent = hex.GetComponent<OwnedHex>();
            hexComponent.woodland = _woodland;
        // }
        List<Tree> trees = _woodland.GetComponent<Woodland>().trees;

        for (int i = 0; i < treesNumber; i++)
        {
            SpawnTree(hexComponent, _woodland.transform, trees);
        }
    }

    public void SpawnTree(OwnedHex hex, Transform _woodland, List<Tree> trees)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(_woodland.transform.position)/* + new Vector3(0, 1f, 0)*/;
        
        float randomScale = Utils.GenerateRandom(0.5f, 1.5f);
        CreateTree(hex, _woodland, position, randomScale, trees);
    }
    
    public void SpawnTreeAt(OwnedHex hex, Transform _woodland, Vector3 position)
    {
        List<Tree> trees = _woodland.GetComponent<Woodland>().trees;
        if (trees.Count <= 100)
        {
            float randomScale = Utils.GenerateRandom(0.3f, 0.8f);
            CreateTree(hex, _woodland, position, randomScale, trees);
        }
    }

    private void CreateTree(OwnedHex hex, Transform _woodland, Vector3 position, float scale, List<Tree> trees)
    {
        float treeRotation = Utils.GenerateRandom(0, 360f);
        GameObject newTree = Instantiate(fruitTree, position, Quaternion.AngleAxis(treeRotation, Vector3.up),
            _woodland.transform);
        newTree.transform.localScale = new Vector3(scale, scale, scale);

        Tree treeComponent = newTree.GetComponent<Tree>();
        trees.Add(treeComponent);
        treeComponent.hex = hex;
    }

    public void ChopTree(GameObject activeHex)
    {
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        if (activeHexComponent.woodland != null)
        {
            Woodland _woodland = activeHexComponent.woodland.GetComponent<Woodland>();

            //choose biggest tree
            GameObject biggestTree = _woodland.ChooseBiggestTree();

            if (biggestTree != null)
            {
                _woodland.ChopTree(biggestTree);
            }
            else
            {
                PlayerMessageService.instance.ShowMessage("No trees to chop! :(");
                Debug.Log("No trees to chop! :(");
            }
        }
        else
        {
            PlayerMessageService.instance.ShowMessage("No trees to chop");
            Debug.Log("No trees to chop");
            return;
        }
    }

    public void CreateConcealedHexesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;

        Vector3[] positionsOfHexesAround = HexUtils.PositionsOfHexesAround(hexPosition);

        foreach (var borderingPosition in positionsOfHexesAround)
        {
            CreateBorderingHexAt(borderingPosition);
        }
    }

    private static void RandomizeResources(GameObject hex)
    {
        BorderingHex borderingHexComponent = hex.GetComponent<BorderingHex>();
        
        borderingHexComponent.hasWater = Utils.TossCoin();
        borderingHexComponent.hasWood = Utils.TossCoin();
    }

    private GameObject CreateBorderingHexAt(Vector3 hexPosition)
    {
        if (!Physics.CheckSphere(hexPosition, overlapRadius))
        {
            // Debug.Log("no obstruction here");
            GameObject newHex = Instantiate(borderingHex, hexPosition, Quaternion.identity, gameObject.transform);
            RandomizeResources(newHex);
            return newHex;
        }
        else
        {
            // Debug.Log("can't spawn hex at" + hexPosition+ ". There's something here");
            return null;
        }
        
    }

    public GameObject ConvertToOwnedHex(BorderingHex borderingHexComponent)
    {
        bool hasWater = borderingHexComponent.hasWater;
        bool hasWood = borderingHexComponent.hasWood;

        //FIXME floating bug here?
        Vector3 position = borderingHexComponent.gameObject.transform.position/* - borderingHexComponent.hoverOffset*/;

        Destroy(borderingHexComponent.gameObject);
        
        GameObject hex = CreateOwnedHex(borderingHexComponent.biome, position);
        if (hasWater)
        {
            SpawnLakes(hex, Utils.GenerateRandomIntMax(5));
        }
        if (hasWood)
        {
            SpawnTrees(hex, Utils.GenerateRandomIntBetween(5, 20));
        }
        return hex;
    }
}
