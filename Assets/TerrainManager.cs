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

    public string treeTag = "tree";
    public float overlapRadius = 1;
    
    public Vector3 firstTileCenter = Vector3.zero;
    public static float HexRadius = 3f;

    public Vector3 xHexOffset = new Vector3(9f, 0f, 0f);
    public Vector3 zHexOffset = new Vector3(0f, 0f, 7.75f);
    
    private void Awake()
    {
        instance = this;
    }

    public GameObject CreateStartHex()
    {
        GameObject startHex = CreateOwnedHex(firstTileCenter);
        SpawnTrees(startHex, StartTrees);
        SpawnLakes(startHex, Utils.GenerateRandomIntMax(5));
        return startHex;
    }

    private GameObject CreateOwnedHex(Vector3 hexCenter)
    {
        GameObject startHex = Instantiate(ownedHex, hexCenter, Quaternion.identity, gameObject.transform);
        string name = NameGenerator.CreateDistrictName();
        startHex.name = name;
        startHex.GetComponent<Hex>().Name = name;
        return startHex;
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
        Vector3 position = ConstructionManager.instance.PositionOnHex(_waterway.transform.position) + new Vector3(0, 0.875f, 0);
        GameObject newLake = Instantiate(lake, position, Quaternion.identity, _waterway.transform);
        float randomScale = Utils.GenerateRandom(0.3f, 1.5f);
        newLake.transform.localScale = new Vector3(randomScale, 1, randomScale);
        
        _waterway.GetComponent<Waterway>().lakes.Add(newLake.GetComponent<Lake>());
    }

    public void SpawnTrees(GameObject hex, int treesNumber)
    {
        // GameObject _woodland = hex.GetComponent<OwnedHex>().woodland;
        // if (woodland == null)
        // {
            GameObject _woodland = Instantiate(woodland, hex.transform.position, Quaternion.identity, hex.transform);
            _woodland.name = "woodland";
            hex.GetComponent<OwnedHex>().woodland = _woodland;
        // }

        for (int i = 0; i < treesNumber; i++)
        {
            SpawnTree(_woodland);
        }
    }

    public void SpawnTree(GameObject _woodland)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(_woodland.transform.position) + new Vector3(0, 1f, 0);
        float treeRotation = Utils.GenerateRandom(0, 360f);
        GameObject newTree = Instantiate(fruitTree, position, Quaternion.AngleAxis(treeRotation, Vector3.up), _woodland.transform);
        float randomScale = Utils.GenerateRandom(0.5f, 1.5f);
        newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        //FIXME ugly and expensive
        _woodland.GetComponent<Woodland>().trees.Add(newTree.GetComponent<Tree>());
    }
    
    public void SpawnTreeAt(Transform _woodland, Vector3 position)
    {
        List<Tree> trees = _woodland.GetComponent<Woodland>().trees;
        if (trees.Count <= 100)
        {
            float treeRotation = Utils.GenerateRandom(0, 360f);
            GameObject newTree = Instantiate(fruitTree, position, Quaternion.AngleAxis(treeRotation, Vector3.up), _woodland.transform);
            float randomScale = Utils.GenerateRandom(0.3f, 0.8f);
            newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            //FIXME ugly and expensive
            trees.Add(newTree.GetComponent<Tree>());
        }
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
                Debug.Log("No trees to chop! :(");
            }
        }
        else
        {
            Debug.Log("No trees to chop");
            return;
        }
    }

    public void CreateConcealedHexesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;
        
        //right
        GameObject rightHex = CreateBorderingHexAt(hexPosition + xHexOffset);

        //left
        GameObject leftHex = CreateBorderingHexAt(hexPosition + -xHexOffset);
        
        //top right
        GameObject toprightHex = CreateBorderingHexAt(hexPosition + xHexOffset/2 + zHexOffset);
        
        //top left
        GameObject toplefttHex = CreateBorderingHexAt(hexPosition + -xHexOffset/2 + zHexOffset);
        
        //bottom right
        GameObject bottomrightHex = CreateBorderingHexAt(hexPosition + xHexOffset/2 - zHexOffset);
        
        //bottom left
        GameObject bottomleft = CreateBorderingHexAt(hexPosition + -xHexOffset/2 - zHexOffset);
    }

    private static void RandomizeResources(GameObject hex)
    {
        BorderingHex borderingHexComponent = hex.GetComponent<BorderingHex>();
        
        borderingHexComponent.hasWater = Utils.TossCoin();
        borderingHexComponent.hasWood = Utils.TossCoin();
        borderingHexComponent.hasFood = Utils.TossCoin();
    }

    private GameObject CreateBorderingHexAt(Vector3 hexPosition)
    {
        var overlapSphere = Physics.OverlapSphere(hexPosition, overlapRadius);
        if (overlapSphere.Length == 0)
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

    public void CreateOwnedHex(GameObject _borderingHex)
    {
        BorderingHex borderingHexComponent = _borderingHex.GetComponent<BorderingHex>();
        bool hasWater = borderingHexComponent.hasWater;
        bool hasWood = borderingHexComponent.hasWood;
        bool hasFood = borderingHexComponent.hasFood;

        Vector3 position = _borderingHex.transform.position;
        Destroy(_borderingHex);

        GameObject hex = CreateOwnedHex(position - borderingHexComponent.hoverOffset);
        if (hasWater)
        {
            SpawnLakes(hex, Utils.GenerateRandomIntMax(5));
        }
        if (hasWood)
        {
            SpawnTrees(hex, Utils.GenerateRandomIntMax(20));
        }
        
        CreateConcealedHexesAround(hex);
        
        GameStats.OwnedHexes++;
    }
}
