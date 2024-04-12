using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    // public int StartTrees;
    
    public GameObject borderingHex;
    public GameObject ownedHex;

    public float overlapRadius = 1;
    
    public Vector3 firstTileCenter = Vector3.zero;
    public static float HexRadius = 3f;
    public int beltWideness;

    //TODO finish list
    public BiomeFeatures BiomeFeaturesGrove;
    public BiomeFeatures BiomeFeaturesForest;
    public BiomeFeatures BiomeFeaturesGrassland;
    public BiomeFeatures BiomeFeaturesEmpty;
    public Dictionary<Biome, BiomeFeatures> BiomeFeaturesDictionary;

    private Biome startBiome = Biome.grove;
    
    private void Awake()
    {
        instance = this;
        BiomeFeaturesDictionary = new Dictionary<Biome, BiomeFeatures>()
        {
            {Biome.grove, BiomeFeaturesGrove},
            {Biome.forest, BiomeFeaturesForest},
            {Biome.grassland, BiomeFeaturesGrassland}
            //TODO finish list
        };
    }
    
    [System.Serializable]
    public class BiomeFeatures
    {
        public LandscapeFeatureType uniqueResource;
        public LandscapeFeatureType secondaryResource;
        public LandscapeFeatureType tertiaryResource;
    }
    
    private BiomeFeatures GetBiomeFeatures(Biome biome)
    {
        /*if (biome != Biome.grove && biome != Biome.forest)
        {
            return BiomeFeaturesEmpty;
        }*/
        return BiomeFeaturesDictionary[biome];
    }

    public GameObject CreateStartHex()
    {
        GameObject startHex = CreateOwnedHex(startBiome, firstTileCenter);
        
        BiomeFeatures biomeFeatures = GetBiomeFeatures(startBiome);
        //TODO modify for another start biomes
        CreateFeature(startHex, biomeFeatures.uniqueResource);
        CreateFeature(startHex, biomeFeatures.secondaryResource);
        
        return startHex;
    }

    private GameObject CreateOwnedHex(Biome biome, Vector3 hexCenter)
    {
        GameObject newOwnedHex = Instantiate(ownedHex, hexCenter, Quaternion.identity, gameObject.transform);
        string districtName = NameGenerator.CreateDistrictName();
        newOwnedHex.name = districtName;
        OwnedHex hexComponent = newOwnedHex.GetComponent<OwnedHex>();
        hexComponent.Name = districtName;

        switch (biome)
        {
            case Biome.grove:
            {
                hexComponent.groveTile.SetActive(true);
                hexComponent.rend = hexComponent.groveTile.GetComponent<Renderer>();
                break;
            }
            case Biome.forest:
            {
                hexComponent.forestTile.SetActive(true);
                hexComponent.rend = hexComponent.forestTile.GetComponent<Renderer>();
                break;
            }
            case Biome.grassland:
            {
                hexComponent.grasslandTile.SetActive(true);
                hexComponent.rend = hexComponent.grasslandTile.GetComponent<Renderer>();
                break;
            }
            /*case Biome.river:
            {
                hexComponent.riverTile.SetActive(true);
                hexComponent.rend = hexComponent.riverTile.GetComponent<Renderer>();
                break;
            }
            case Biome.swamp:
            {
                hexComponent.swampTile.SetActive(true);
                hexComponent.rend = hexComponent.swampTile.GetComponent<Renderer>();
                break;
            }
            case Biome.mountain:
            {
                hexComponent.mountainTile.SetActive(true);
                hexComponent.rend = hexComponent.mountainTile.GetComponent<Renderer>();
                break;
            }*/
            default:
            {
                Debug.Log("no tile for this biome");
                break;
            }
        }
        hexComponent.biome = biome;
        
        //TODO create features
        
        
        return newOwnedHex;
    }
    
    private void CreateFeature(GameObject hex, LandscapeFeatureType landscapeFeatureType)
    {
        if (landscapeFeatureType == LandscapeFeatureType.none)
        {
            return;
        }
        
        //get blueprint for feature
        LandscapeFeatureBlueprint featureBlueprint = LandscapeFeaturesDictionary.GetLandscapeFeatureBlueprint(landscapeFeatureType);

        //create instance of feature
        LandscapeFeature feature = null;
        switch (featureBlueprint.landscapeFeatureType)
        {
            case LandscapeFeatureType.fruitTrees:
            {
                feature = new LandscapeFeatureGrove();
                break;
            }
            case LandscapeFeatureType.lakes:
            {
                feature = new LandscapeFeatureLakes();
                break;
            }
            case LandscapeFeatureType.pineTrees:
            {
                feature = new LandscapeFeatureForest();
                break;
            }
            case LandscapeFeatureType.field:
            {
                feature = new LandscapeFeatureFields();
                break;
            }
            default:
            {
                Debug.Log("Unknown landscape feature");
                break;
            }
        }
        
        //assign feature to Hex
        feature.AssignToTile(hex);
        
        //spawn feature resources
        for (int i = 0; i < featureBlueprint.resourceMaxCount; i++)
        {
            SpawnResource(feature, featureBlueprint.resourceGO, hex);
        } 
    }

    private void SpawnResource(LandscapeFeature landscapeFeature, GameObject resourcePrefab, GameObject parent)
    {
        Vector3 position = ConstructionManager.instance.PositionOnHex(parent.transform.position);
        float rotation = Utils.GenerateRandom(0, 360f);
        GameObject resource = Instantiate(resourcePrefab, position, Quaternion.AngleAxis(rotation, Vector3.up), parent.transform);
        //TODO test scale for different resources
        float randomScale = Utils.GenerateRandom(0.9f, 1.1f);
        resource.transform.localScale = new Vector3(randomScale, 1, randomScale);
        
        landscapeFeature.AddResource(resource);
    }
    

    public void SpawnTreeAt(OwnedHex hex, GameObject treePrefab, Vector3 position)
    {
        //TODO spawn tree of random size
        float randomScale = Utils.GenerateRandom(0.2f, 0.3f);
        SpawnResource(hex.GetWoodland(), treePrefab, gameObject);
    }

    public void ChopTree(GameObject activeHex)
    {
        OwnedHex activeHexComponent = activeHex.GetComponent<OwnedHex>();
        
        LandscapeFeatureWoodland landscapeFeatureWoodland = activeHexComponent.GetWoodland();
        
        GameObject biggestTree = landscapeFeatureWoodland.ChooseBiggestTree();
            
        if (biggestTree != null)
        {
            landscapeFeatureWoodland.ChopTree(biggestTree);
        }
        else
        {
            PlayerMessageService.instance.ShowMessage("No trees to chop! :(");
            Debug.Log("No trees to chop! :(");
        }
    }

    public void CreateConcealedHexesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;

        Vector3[] positionsOfHexesAround = HexUtils.PositionsOfHexesAround(hexPosition);

        foreach (var borderingPosition in positionsOfHexesAround)
        {
            CreateBorderingHexAt(borderingPosition);
        }
    }

    /*private static void RandomizeResources(BorderingHex borderingHexComponent)
    {
        borderingHexComponent.hasWater = Utils.TossCoin();
        borderingHexComponent.hasWood = Utils.TossCoin();
    }*/

    private static void RandomizeFeatures(BorderingHex borderingHexComponent)
    {
        borderingHexComponent.hasUniqueResource = Utils.TossCoin();
        borderingHexComponent.hasSecondaryResource = Utils.TossCoin();
        borderingHexComponent.hasTertiaryResource = Utils.TossCoin();
        borderingHexComponent.hasRestriction = Utils.TossCoin();
    }

    private GameObject CreateBorderingHexAt(Vector3 hexPosition)
    {
        if (!Physics.CheckSphere(hexPosition, overlapRadius))
        {
            // Debug.Log("no obstruction here");
            GameObject newHex = Instantiate(borderingHex, hexPosition, Quaternion.identity, gameObject.transform);
            BorderingHex newHexComponent = newHex.GetComponent<BorderingHex>();

            DefineBiome(hexPosition, newHexComponent);
            // RandomizeResources(newHexComponent);
            RandomizeFeatures(newHexComponent);

            return newHex;
        }
        else
        {
            // Debug.Log("can't spawn hex at" + hexPosition+ ". There's something here");
            return null;
        }
        
    }

    private void DefineBiome(Vector3 hexPosition, BorderingHex newHexComponent)
    {
        // Vector3[] positionsOfHexesAround = HexUtils.PositionsOfHexesAround(hexPosition);
        List<OwnedHex> hexesAround = newHexComponent.GetOwnedHexesAround();
        int randomHexIndex = Utils.GenerateRandomIntMax(6);

        Hex randomHex = hexesAround.ElementAtOrDefault(randomHexIndex - 1);

        Biome biome;
        if (randomHex != null)
        {
            biome = randomHex.biome;
        }
        else
        {
            biome = GetHexBiomeByPosition(hexPosition);
        }

        newHexComponent.biome = biome;
    }

    private Biome GetHexBiomeByPosition(Vector3 hexPosition)
    {
        // float aaaaa = hexPosition.z / HexUtils.zHexOffset.z;
        int beltIndex = (int)(hexPosition.z / HexUtils.zHexOffset.z / beltWideness);
        // Debug.Log(beltIndex);

        Biome biome = RandomBiomeByBelt(beltIndex);
        
        return biome;
    }

    private Biome RandomBiomeByBelt(int beltIndex)
    {
        switch (beltIndex)
        {
            case 0:
            {
                // return Utils.TossCoin() ? Biome.grove : Biome.river;
                return Biome.grove;
            }
            case 1:
            {                
                // return Utils.TossCoin() ? Biome.forest : Biome.mountain;
                return Biome.forest;
            }
            case -1:
            {
                // return Utils.TossCoin() ? Biome.grassland : Biome.swamp;
                return Biome.grassland;
            }
            //TODO add others
            default:
            {
                Biome[] biomes = Enum.GetValues(typeof(Biome)) as Biome[];
                if (beltIndex > 0)
                {
                    return biomes[0];
                } else if (beltIndex < 0)
                {
                    return biomes[biomes.Length-1];
                }
                else
                {
                    Debug.Log("DEFAULT BIOME");
                    return Biome.grove;
                }
            }
        }
    }

    public GameObject ConvertToOwnedHex(BorderingHex borderingHexComponent)
    {
        Biome biome = borderingHexComponent.biome;
        
        // bool hasWater = borderingHexComponent.hasWater;
        // bool hasWood = borderingHexComponent.hasWood;
        
        bool hasUniqueResource = borderingHexComponent.hasUniqueResource;
        bool hasSecondaryResource = borderingHexComponent.hasSecondaryResource;
        bool hasTertiaryResource = borderingHexComponent.hasTertiaryResource;
        bool hasRestriction = borderingHexComponent.hasRestriction;
        
        //change tilePrefab based on biome
        Vector3 position = borderingHexComponent.gameObject.transform.position;
        Destroy(borderingHexComponent.gameObject);
        GameObject hex = CreateOwnedHex(biome, position);
        
        //create features based on biome

        BiomeFeatures biomeFeatures = GetBiomeFeatures(biome);
        if (hasUniqueResource)
        {
            // CreateFeature(hex, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.uniqueResource).resourceGO, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.uniqueResource).resourceMaxCount);
            CreateFeature(hex, biomeFeatures.uniqueResource);
        }
        if (hasSecondaryResource)
        {
            // CreateFeature(hex, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.secondaryResource).resourceGO, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.secondaryResource).resourceMaxCount);
            CreateFeature(hex, biomeFeatures.secondaryResource);
        }
        if (hasTertiaryResource)
        {
            // CreateFeature(hex, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.tertiaryResource).resourceGO, LandscapeFeaturesDictionary.GetLandscapeFeature(biomeFeatures.tertiaryResource).resourceMaxCount);
            CreateFeature(hex, biomeFeatures.tertiaryResource);
        }
        //TODO
        /*if (hasRestriction)
        {
            CreateFeature(hex, biomeFeatures.Restriction, biomeFeatures.RestrictionMaxCount);
        }*/
        
        return hex;
    }
}
