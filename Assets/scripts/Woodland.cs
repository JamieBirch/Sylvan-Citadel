using System.Collections.Generic;
using UnityEngine;

public class Woodland : MonoBehaviour
{
    public List<Tree> trees;

    private void Start()
    {
        if (trees.Count == 0)
        {
            trees = new List<Tree>();
        }
    }
    
    public GameObject ChooseBiggestTree()
    {
        Tree biggestTree = null;

        foreach (Tree _tree in trees)
        {
            if (biggestTree == null)
            {
                biggestTree = _tree;
            }

            float sizeDifference = biggestTree.treeSize.GetSize() - _tree.treeSize.GetSize();
            if (sizeDifference < 0)
            {
                biggestTree = _tree;
            }
        }

        return biggestTree.gameObject;
    }
    
    public void ChopTree(GameObject _tree)
    {
        Tree treeComponent = _tree.GetComponent<Tree>();

        trees.Remove(treeComponent);
        Destroy(_tree);
        int woodAmount = (int)treeComponent.treeSize.GetSize();
        Debug.Log("chop tree, " + woodAmount);
        GameStats.instance.AddWood(woodAmount);
    }
}
