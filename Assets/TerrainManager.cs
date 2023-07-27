using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    public int StartTrees;
    
    public GameObject borderingHex;
    public GameObject farHex;
    public GameObject ownedHex;

    public GameObject woodland;
    public GameObject fruitTree;
    public GameObject pineTree;
    public GameObject lake;

    public string treeTag = "tree";
    
    public Vector3 firstTileCenter = Vector3.zero;
    public static float HexRadius = 3f;

    public Vector3 xHexOffset = new Vector3(9f, 0f, 0f);
    public Vector3 zHexOffset = new Vector3(0f, 0f, 7.75f);
    
    private void Awake()
    {
        instance = this;
    }

    /*public void CreateTerrain()
    {
        //instantiate start hex
        // CreateStartHex();

        //TODO instantiate bordering hexes

        //TODO instantiate far hexes
    }*/

    public GameObject CreateStartHex()
    {
        GameObject startHex = Instantiate(ownedHex, firstTileCenter, Quaternion.identity, gameObject.transform);
        string name = NameGenerator.CreateDistrictName();
        startHex.name = name;
        startHex.GetComponent<Hex>().Name = name;
        SpawnTrees(startHex, StartTrees);
        SpawnLake(startHex);
        return startHex;
    }

    private void SpawnLake(GameObject hex)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(hex.transform.position) + new Vector3(0, 0.875f, 0);
        GameObject _lake = Instantiate(lake, position, Quaternion.identity, hex.transform);
        float randomScale = Utils.GenerateRandom(0.5f, 1f);
        _lake.transform.localScale = new Vector3(randomScale, 1, randomScale);

        hex.GetComponent<OwnedHex>().waterway = _lake;
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
            GameObject newTree = SpawnTree(_woodland);
            newTree.transform.SetParent(_woodland.transform);
        }
    }

    public GameObject SpawnTree(GameObject woodland)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(woodland.transform.position) + new Vector3(0, 1f, 0);
        GameObject newTree = Instantiate(fruitTree, position, Quaternion.identity, woodland.transform);
        float randomScale = Utils.GenerateRandom(0.5f, 1.5f);
        newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        return newTree;
    }
    
    public void SpawnTreeAt(Transform woodland, Vector3 position)
    {
        GameObject newTree = Instantiate(fruitTree, position, Quaternion.identity, woodland);
        float randomScale = Utils.GenerateRandom(0.3f, 0.8f);
        newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    public void ChopTree()
    {
        //choose biggest tree
        GameObject biggestTree = null;
        GameObject[] trees = GameObject.FindGameObjectsWithTag(treeTag);
        foreach (GameObject _tree in trees)
        {
            if (biggestTree == null)
            {
                biggestTree = _tree;
            }

            float sizeDifference = biggestTree.GetComponent<Tree>().size - _tree.GetComponent<Tree>().size;
            if (sizeDifference < 0)
            {
                biggestTree = _tree;
            }
        }

        if (biggestTree != null)
        {
            //chop tree
            Destroy(biggestTree);
            int woodAmount = (int)biggestTree.GetComponent<Tree>().size;
            Debug.Log("chop tree, " + woodAmount);
            GameStats.Wood += woodAmount;
        }
        else
        {
            Debug.Log("No trees to chop! :(");
        }

    }

    public void CreateConcealedHexesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;
        
        //right
        GameObject rightHex = CreateBorderingHexAt(hexPosition + xHexOffset);
        RandomizeResources(rightHex);

        //left
        GameObject leftHex = CreateBorderingHexAt(hexPosition + -xHexOffset);
        RandomizeResources(leftHex);
        
        //top right
        GameObject toprightHex = CreateBorderingHexAt(hexPosition + xHexOffset/2 + zHexOffset);
        RandomizeResources(toprightHex);
        
        //top left
        GameObject toplefttHex = CreateBorderingHexAt(hexPosition + -xHexOffset/2 + zHexOffset);
        RandomizeResources(toplefttHex);
        
        //bottom right
        GameObject bottomrightHex = CreateBorderingHexAt(hexPosition + xHexOffset/2 - zHexOffset);
        RandomizeResources(bottomrightHex);
        
        //bottom left
        GameObject bottomleft = CreateBorderingHexAt(hexPosition + -xHexOffset/2 - zHexOffset);
        RandomizeResources(bottomleft);
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
        var overlapSphere = Physics.OverlapSphere(hexPosition, 1);
        if (overlapSphere.Length == 0)
        {
            // Debug.Log("no obstruction here");
            GameObject newHex = Instantiate(borderingHex, hexPosition, Quaternion.identity, gameObject.transform);
            return newHex;
        }
        else
        {
            Debug.Log("can't spawn hex at" + hexPosition+ ". There's something here");
            return null;
        }
    }
}
