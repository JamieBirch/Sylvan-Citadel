using System.Collections.Generic;
using UnityEngine;

public class LandscapeFeaturesDictionary : MonoBehaviour
{
    public static Dictionary<LandscapeFeatureType, LandscapeFeature> LandscapeFeatureDictionary;

    public LandscapeFeature LandscapeFeatureForest;
    public LandscapeFeature LandscapeFeatureGrassland;
    public LandscapeFeature LandscapeFeatureGrove;
    public LandscapeFeature LandscapeFeatureLakes;
    
    private void Awake()
    {
        LandscapeFeatureDictionary = new Dictionary<LandscapeFeatureType, LandscapeFeature>()
        {
            {LandscapeFeatureType.field, LandscapeFeatureGrassland},
            {LandscapeFeatureType.lakes, LandscapeFeatureLakes},
            {LandscapeFeatureType.fruitTrees, LandscapeFeatureGrove},
            {LandscapeFeatureType.pineTrees, LandscapeFeatureForest}
            //TODO finish list
        };
    }

    public static LandscapeFeature GetLandscapeFeature(LandscapeFeatureType type)
    {
        return LandscapeFeatureDictionary[type];
    }
    
}
