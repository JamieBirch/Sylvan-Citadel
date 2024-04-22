using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandscapeFeatureBlueprint
{
    public LandscapeFeatureType landscapeFeatureType;
    public GameObject resourceGO;
    public int resourceMinCount;
    public int resourceMaxCount;

    /*public LandscapeFeature(LandscapeFeatureType landscapeFeatureType, GameObject resourceGO, int resourceMaxCount)
    {
        this.landscapeFeatureType = landscapeFeatureType;
        this.resourceGO = resourceGO;
        this.resourceMaxCount = resourceMaxCount;
    }*/
}
