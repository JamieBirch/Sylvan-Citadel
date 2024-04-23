using System.Collections.Generic;
using UnityEngine;

public class LandscapeFeaturesDictionary : MonoBehaviour
{
    public static Dictionary<LandscapeFeatureType, LandscapeFeatureBlueprint> LandscapeFeatureDictionary;

    public LandscapeFeatureBlueprint landscapeFeatureBlueprintForest;
    public LandscapeFeatureBlueprint landscapeFeatureBlueprintField;
    public LandscapeFeatureBlueprint landscapeFeatureBlueprintGrove;
    public LandscapeFeatureBlueprint landscapeFeatureBlueprintLakes;
    
    private void Awake()
    {
        LandscapeFeatureDictionary = new Dictionary<LandscapeFeatureType, LandscapeFeatureBlueprint>()
        {
            {LandscapeFeatureType.field, landscapeFeatureBlueprintField},
            {LandscapeFeatureType.lakes, landscapeFeatureBlueprintLakes},
            {LandscapeFeatureType.fruitTrees, landscapeFeatureBlueprintGrove},
            {LandscapeFeatureType.pineTrees, landscapeFeatureBlueprintForest}
            //TODO finish list
        };
    }

    public static LandscapeFeatureBlueprint GetLandscapeFeatureBlueprint(LandscapeFeatureType type)
    {
        return LandscapeFeatureDictionary[type];
    }
    
}
