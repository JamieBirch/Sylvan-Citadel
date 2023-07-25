using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    public int StartTrees;
    
    public GameObject borderingHex;
    public GameObject farHex;
    public GameObject ownedHex;
    
    public GameObject lake;
    public GameObject tree;
    
    public string treeTag = "tree";
    
    public Vector3 firstTileCenter = Vector3.zero;
    public static float HexRadius = 3f;

    public Vector3 xHexOffset = new Vector3(9f, 0f, 0f);
    public Vector3 zHexOffset = new Vector3(0f, 0f, 7.75f);
    
    private void Awake()
    {
        instance = this;
    }

    public void CreateTerrain()
    {
        //instantiate start hex
        GameObject startHex = Instantiate(ownedHex, firstTileCenter, Quaternion.identity, gameObject.transform);
        string name = NameGenerator.CreateDistrictName();
        startHex.name = name;
        startHex.GetComponent<Hex>().Name = name;
        //TODO instantiate trees and lake ON START HEX
        SpawnTrees(startHex, StartTrees);
        SpawnLake(startHex);

        //TODO instantiate bordering hexes

        //TODO instantiate far hexes
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
        GameObject woodland = hex.GetComponent<OwnedHex>().woodland;
        if (woodland == null)
        {
            woodland = new GameObject("woodland");
            woodland.transform.SetParent(hex.transform);
        }

        for (int i = 0; i < treesNumber; i++)
        {
            GameObject newTree = SpawnTree(hex.transform.position);
            newTree.transform.SetParent(woodland.transform);
        }
    }

    public GameObject SpawnTree(Vector3 tileCenter)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(tileCenter) + new Vector3(0, 1f, 0);
        GameObject newTree = Instantiate(tree, position, Quaternion.identity);
        float randomScale = Utils.GenerateRandom(0.5f, 1.5f);
        newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        return newTree;
    }
    
    public void SpawnTreeAt(Vector3 position)
    {
        GameObject newTree = Instantiate(tree, position, Quaternion.identity);
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
}
