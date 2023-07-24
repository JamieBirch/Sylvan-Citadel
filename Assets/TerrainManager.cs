using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    public GameObject concealedHex;
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
    
    public void SpawnTrees(int treesNumber)
    {
        for (int i = 0; i < treesNumber; i++)
        {
            SpawnTree();
        }
    }

    public void SpawnTree()
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(firstTileCenter) + new Vector3(0, 1f, 0);
        GameObject newTree = Instantiate(tree, position, Quaternion.identity);
        float randomScale = Utils.GenerateRandom(0.5f, 1.5f);
        newTree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
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

            float distanceToWater = Vector3.Distance(biggestTree.transform.localScale, _tree.transform.localScale);
            if (distanceToWater < 0)
            {
                biggestTree = _tree;
            }
        }
        
        //chop tree
        Destroy(biggestTree);
        int woodAmount = (int)(biggestTree.transform.localScale.magnitude * 10);
        Debug.Log("chop tree, " + woodAmount);
        GameStats.Wood += woodAmount;
    }
}
