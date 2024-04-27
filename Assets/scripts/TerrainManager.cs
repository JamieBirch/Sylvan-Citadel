using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    
    public GameObject fogTile;
    public GameObject borderingHex;
    public GameObject ownedHex;

    public float overlapRadius = 1;
    
    public Vector3 firstTileCenter = Vector3.zero;
    public int beltWideness;

    //TODO finish list
    public BiomeFeatures BiomeFeaturesGrove;
    public BiomeFeatures BiomeFeaturesForest;
    public BiomeFeatures BiomeFeaturesGrassland;
    public Dictionary<Biome, BiomeFeatures> BiomeFeaturesDictionary;

    private Biome startBiome = Biome.grove;
    
    public TileStatsUIContainer tileStatsUIcontainer;
    
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
    
    public GameObject CreateStartTile()
    {
        GameObject startHex = CreateOwnedTile(startBiome, firstTileCenter);
        
        BiomeFeatures biomeFeatures = BiomeFeaturesDictionary[startBiome];
        //TODO modify for another start biomes
        CreateFeature(startHex, biomeFeatures.uniqueResource);
        CreateFeature(startHex, biomeFeatures.secondaryResource);
        
        return startHex;
    }

    private GameObject CreateOwnedTile(Biome biome, Vector3 tileCenter)
    {
        GameObject newOwnedTile = Instantiate(ownedHex, tileCenter, Quaternion.identity, gameObject.transform);
        string districtName = NameGenerator.CreateDistrictName();
        newOwnedTile.name = districtName;
        OwnedHex tileComponent = newOwnedTile.GetComponent<OwnedHex>();
        tileComponent.Name = districtName;

        TileStatsUI tileStatsUI = Instantiate(tileComponent.tileStatsUIprefab, tileStatsUIcontainer.transform).GetComponent<TileStatsUI>();
        tileComponent.tileStatsUI = tileStatsUI;
        tileStatsUI.gameObject.SetActive(false);

    switch (biome)
        {
            case Biome.grove:
            {
                tileComponent.groveTile.SetActive(true);
                tileComponent.rend = tileComponent.groveTile.GetComponent<Renderer>();
                break;
            }
            case Biome.forest:
            {
                tileComponent.forestTile.SetActive(true);
                tileComponent.rend = tileComponent.forestTile.GetComponent<Renderer>();
                break;
            }
            case Biome.grassland:
            {
                tileComponent.grasslandTile.SetActive(true);
                tileComponent.rend = tileComponent.grasslandTile.GetComponent<Renderer>();
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
        tileComponent.biome = biome;
        
        GameStats.AddTile(tileComponent);
        
        return newOwnedTile;
    }
    
    private void CreateFeature(GameObject tile, LandscapeFeatureType landscapeFeatureType)
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
        feature.AssignToTile(tile);

        int randomCount = Utils.GenerateRandomIntBetween(featureBlueprint.resourceMinCount, featureBlueprint.resourceMaxCount);

        //spawn feature resources
        for (int i = 0; i < randomCount; i++)
        {
            Vector3 position = TileUtils.PositionOnTile(tile.transform.position);
            SpawnResource(feature, featureBlueprint.resourceGO, tile, position);
        } 
    }

    private void SpawnResource(LandscapeFeature landscapeFeature, GameObject resourcePrefab, GameObject tile, Vector3 position)
    {
        // Vector3 position = TileUtils.PositionOnTile(tile.transform.position);
        float rotation = Utils.GenerateRandom(0, 360f);
        GameObject resource = Instantiate(resourcePrefab, position, Quaternion.AngleAxis(rotation, Vector3.up), tile.transform);
        //TODO test scale for different resources
        float randomScale = Utils.GenerateRandom(0.9f, 1.1f);
        resource.transform.localScale = new Vector3(randomScale, 1, randomScale);
        
        landscapeFeature.AddResource(resource);
    }
    

    public void SpawnTreeAt(OwnedHex hex, GameObject treePrefab, Vector3 position)
    {
        //TODO spawn tree of random size
        float randomScale = Utils.GenerateRandom(0.2f, 0.3f);
        SpawnResource(hex.GetWoodland(), treePrefab, hex.gameObject, position);
    }

    public void CreateConcealedHexesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;

        Vector3[] positionsOfHexesAround = TileUtils.PositionsOfHexesAround(hexPosition);

        List<GameObject> borderingTiles = new List<GameObject>();
        foreach (var borderingPosition in positionsOfHexesAround)
        {
            GameObject borderingTile = CreateBorderingHexAt(borderingPosition);
            if (borderingTile != null)
            {
                borderingTiles.Add(borderingTile);
            }
        }
        
        foreach (var borderingTile in borderingTiles)
        {
            Vector3[] positionsOfTilesAround = TileUtils.PositionsOfHexesAround(borderingTile.transform.position);
            foreach (var tilePosition in positionsOfTilesAround)
            {
                CreateConcealedTileAt(tilePosition);
            }
        }
    }

    private static void RandomizeFeatures(BorderingHex borderingHexComponent)
    {
        borderingHexComponent.hasUniqueResource = Utils.TossCoin();
        borderingHexComponent.hasSecondaryResource = Utils.TossCoin();
        borderingHexComponent.hasTertiaryResource = Utils.TossCoin();
        borderingHexComponent.hasRestriction = Utils.TossCoin();
    }

    private GameObject CreateBorderingHexAt(Vector3 hexPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(hexPosition, overlapRadius);
        if (colliders.Length == 0)
        {
            return CreateNewBorderingTile(hexPosition);
        }
        else if (collidesWithFogTile(colliders))
        {
            foreach(var collider in colliders)
            {
                var go = collider.gameObject; //This is the game object you collided with
                if (go.TryGetComponent<ConcealedHex>(out _))
                {
                    Destroy(go);
                }
            }

            return CreateNewBorderingTile(hexPosition);
        } 
        else
        {
            // Debug.Log("can't spawn hex at" + hexPosition+ ". There's something here");
            return null;
        }
    }

    private GameObject CreateNewBorderingTile(Vector3 position)
    {
        GameObject newHex = Instantiate(borderingHex, position, Quaternion.identity, gameObject.transform);
        BorderingHex newHexComponent = newHex.GetComponent<BorderingHex>();

        DefineBiome(position, newHexComponent);
        RandomizeFeatures(newHexComponent);
        return newHexComponent.gameObject;
    }

    private bool collidesWithFogTile(Collider[] colliders)
    {
        foreach(var collider in colliders)
        {
            var go = collider.gameObject; //This is the game object you collided with
            if (go.TryGetComponent<ConcealedHex>(out _))
            {
                return true;
            }
        }

        return false;
    }

    private GameObject CreateConcealedTileAt(Vector3 tilePosition)
    {
        if (!Physics.CheckSphere(tilePosition, overlapRadius))
        {
            // Debug.Log("no obstruction here");
            GameObject newHex = Instantiate(fogTile, tilePosition, Quaternion.identity, gameObject.transform);
            // BorderingHex newHexComponent = newHex.GetComponent<BorderingHex>();

            // DefineBiome(tilePosition, newHexComponent);
            // RandomizeResources(newHexComponent);
            // RandomizeFeatures(newHexComponent);

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
        int randomHexIndex = Utils.GenerateRandomIntNumberWhereMaxIs(6);

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
        int beltIndex = (int)(hexPosition.z / TileUtils.zHexOffset.z / beltWideness);
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
        
        bool hasUniqueResource = borderingHexComponent.hasUniqueResource;
        bool hasSecondaryResource = borderingHexComponent.hasSecondaryResource;
        bool hasTertiaryResource = borderingHexComponent.hasTertiaryResource;
        bool hasRestriction = borderingHexComponent.hasRestriction;
        
        //change tilePrefab based on biome
        Vector3 position = borderingHexComponent.gameObject.transform.position;
        Destroy(borderingHexComponent.gameObject);
        GameObject hex = CreateOwnedTile(biome, position);
        SoundManager.PlaySound(SoundManager.Sound.new_tile);
        
        //create features based on biome
        BiomeFeatures biomeFeatures = BiomeFeaturesDictionary[biome];
        if (hasUniqueResource)
        {
            CreateFeature(hex, biomeFeatures.uniqueResource);
        }
        if (hasSecondaryResource)
        {
            CreateFeature(hex, biomeFeatures.secondaryResource);
        }
        if (hasTertiaryResource)
        {
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