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

            float sizeDifference = biggestTree.size - _tree.size;
            if (sizeDifference < 0)
            {
                biggestTree = _tree;
            }
        }

        return biggestTree.gameObject;
    }
}
