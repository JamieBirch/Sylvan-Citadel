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
    
    public void GrowNewTree(TerrainManager terrainManager)
    {
        int treesOnTile = getCount();
        if (treesOnTile < 100)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance < 100 - treesOnTile)
            {
                terrainManager.SpawnTree(this, tile.gameObject);
            }
        }
    }
    
    public GameObject ChooseBiggestTree()
    {
        Tree biggestTree = null;

        if (trees.Count == 0)
        {
            Debug.Log("Tree list empty");
            PlayerMessageService.instance.ShowMessage("No trees to chop!");
            return null;
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
        GameStats.instance.AddWood(woodAmount);
        SoundManager.PlaySound(SoundManager.Sound.chop);
    }

    public override void AddResource(GameObject go)
    {
        Tree treeComponent = go.GetComponent<Tree>();
        treeComponent.tile = tile;
        trees.Add(treeComponent);
    }

    public GameObject GetTreePrefab()
    {
        return LandscapeFeaturesDictionary.GetLandscapeFeatureBlueprint(getFeatureType()).resourceGO;
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
        Field fieldComponent = go.GetComponent<Field>();
        fields.Add(fieldComponent);
    }

    public override LandscapeFeatureType getFeatureType()
    {
        return LandscapeFeatureType.field;
    }
}