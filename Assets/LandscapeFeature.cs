using System.Collections.Generic;
using UnityEngine;

public abstract class LandscapeFeature
{
    public OwnedHex tile;
    
    public abstract void AddResource(GameObject go);

    public void AssignToTile(GameObject hex)
    {
        tile = hex.GetComponent<OwnedHex>();
        tile.LandscapeFeaturesDictionary.Add(getFeatureType(), this);
        // tile.tileStatistics.Add(getFeatureType().ToString(), 0);
    }

    public abstract LandscapeFeatureType getFeatureType();

    public abstract int getCount();

}

public abstract class LandscapeFeatureWoodland : LandscapeFeature
{
    public List<Tree> trees = new List<Tree>();

    public override int getCount()
    {
        return trees.Count;
    }
    
    public GameObject ChooseBiggestTree()
    {
        Tree biggestTree = null;

        if (trees.Count == 0)
        {
            Debug.Log("Tree list empty");
        }

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
        int woodAmount = (int)treeComponent.treeSize.GetSize();
        treeComponent.Chop();
        Debug.Log("chop tree, " + woodAmount);
        GameStats.Wood += woodAmount;
    }

    public override void AddResource(GameObject go)
    {
        //TODO new tree
        
        Tree treeComponent = go.GetComponent<Tree>();
        treeComponent.hex = tile;
        trees.Add(treeComponent);
    }
}

public class LandscapeFeatureForest : LandscapeFeatureWoodland
{
    public override LandscapeFeatureType getFeatureType()
    {
        return LandscapeFeatureType.pineTrees;
    }
}

public class LandscapeFeatureGrove : LandscapeFeatureWoodland
{
    public override LandscapeFeatureType getFeatureType()
    {
        return LandscapeFeatureType.fruitTrees;
    }
}

public class LandscapeFeatureLakes : LandscapeFeature
{
    public List<Lake> lakes = new List<Lake>();
    
    public override int getCount()
    {
        return lakes.Count;
    }
    
    public override void AddResource(GameObject go)
    {
        //TODO new Lake
        
        Lake lakeComponent = go.GetComponent<Lake>();
        lakes.Add(lakeComponent);
    }

    public override LandscapeFeatureType getFeatureType()
    {
        return LandscapeFeatureType.lakes;
    }
}

public class LandscapeFeatureFields : LandscapeFeature
{
    public List<Field> fields = new List<Field>();
    
    public override int getCount()
    {
        return fields.Count;
    }
    
    public override void AddResource(GameObject go)
    {
        //TODO new Lake
        
        Field fieldComponent = go.GetComponent<Field>();
        fields.Add(fieldComponent);
    }

    public override LandscapeFeatureType getFeatureType()
    {
        return LandscapeFeatureType.field;
    }
}