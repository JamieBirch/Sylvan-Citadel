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
    
    //decorations
    public GameObject rockPrefab;
    public GameObject stonePrefab;

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

    public Dictionary<Biome, int> borderingTilesCounter = new Dictionary<Biome, int>();

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
        OwnedTile tileComponent = newOwnedTile.GetComponent<OwnedTile>();
        tileComponent.Name = districtName;

        TileStatsUI tileStatsUI = Instantiate(tileComponent.tileStatsUIprefab, tileStatsUIcontainer.transform).GetComponent<TileStatsUI>();
        tileComponent.tileStatsUI = tileStatsUI;
        tileComponent.tileBuildingsUI = tileStatsUI.tileBuildings;
        tileStatsUI.gameObject.SetActive(false);


        GameObject tileGroundLayers = null;
        switch (biome)
        {
            case Biome.grove:
            {
                tileGroundLayers = tileComponent.groveLayers;
                // tileComponent.groveLayers.SetActive(true);
                // tileComponent.rend = tileComponent.groveTile.GetComponent<Renderer>();
                break;
            }
            case Biome.forest:
            {
                tileGroundLayers = tileComponent.forestLayers;
                // tileComponent.forestTile.SetActive(true);
                // tileComponent.rend = tileComponent.forestTile.GetComponent<Renderer>();
                break;
            }
            case Biome.grassland:
            {
                tileGroundLayers = tileComponent.grasslandLayers;
                // tileComponent.grasslandTile.SetActive(true);
                // tileComponent.rend = tileComponent.grasslandTile.GetComponent<Renderer>();
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
        tileGroundLayers.SetActive(true);
        RandomizeTileGroundLandscape(tileGroundLayers.GetComponent<GroundLayers>());
        
        tileComponent.biome = biome;
        
        GameStats.AddTile(tileComponent);
        
        //TODO WTF??
        tileComponent.tileWall.SurroundNewTile(tileComponent);
        
        return newOwnedTile;
    }

    private void RandomizeTileGroundLandscape(GroundLayers tileGroundLayers)
    {
        foreach (GameObject layer in tileGroundLayers.layers)
        {
            float yAngle = 60* Utils.GenerateRandomIntNumberWhereMaxIs(6);
            layer.transform.Rotate(0, 0, yAngle);
        }
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

    public void SpawnTree(LandscapeFeatureWoodland woodland, GameObject tile)
    {
        Vector3 position = TileUtils.PositionOnTile(tile.transform.position);
        SpawnResource(woodland, woodland.GetTreePrefab(), tile, position);
    }

    private void SpawnResource(LandscapeFeature landscapeFeature, GameObject resourcePrefab, GameObject tile, Vector3 position)
    {
        // Vector3 position = TileUtils.PositionOnTile(tile.transform.position);
        var resource = CreateInTile(resourcePrefab, tile, position);
        landscapeFeature.AddResource(resource);
    }

    private static GameObject CreateInTile(GameObject resourcePrefab, GameObject tile, Vector3 position)
    {
        float rotation = Utils.GenerateRandom(0, 360f);
        GameObject resource = Instantiate(resourcePrefab, position, Quaternion.AngleAxis(rotation, Vector3.up), tile.transform);
        //TODO test scale for different resources
        float randomScale = Utils.GenerateRandom(0.6f, 1.2f);
        resource.transform.localScale = new Vector3(randomScale, 1, randomScale);
        return resource;
    }


    public void SpawnTreeAt(OwnedTile tile, GameObject treePrefab, Vector3 position)
    {
        //TODO spawn tree of random size
        float randomScale = Utils.GenerateRandom(0.2f, 0.3f);
        SpawnResource(tile.GetWoodland(), treePrefab, tile.gameObject, position);
    }

    public void CreateConcealedTilesAround(GameObject hex)
    {
        Vector3 hexPosition = hex.transform.position;

        Vector3[] positionsOfHexesAround = TileUtils.PositionsOfTilesAround(hexPosition);

        List<GameObject> borderingTiles = new List<GameObject>();
        foreach (var borderingPosition in positionsOfHexesAround)
        {
            GameObject borderingTile = CreateBorderingTileAt(borderingPosition);
            if (borderingTile != null)
            {
                borderingTiles.Add(borderingTile);
            }
        }
        
        foreach (var borderingTile in borderingTiles)
        {
            Vector3[] positionsOfTilesAround = TileUtils.PositionsOfTilesAround(borderingTile.transform.position);
            foreach (var tilePosition in positionsOfTilesAround)
            {
                CreateConcealedTileAt(tilePosition);
            }
        }
    }

    private static void RandomizeFeatures(BorderingTile borderingTileComponent)
    {
        borderingTileComponent.hasUniqueResource = Utils.TossCoin();
        borderingTileComponent.hasSecondaryResource = Utils.TossCoin();
        borderingTileComponent.hasTertiaryResource = Utils.TossCoin();
        borderingTileComponent.hasRestriction = Utils.TossCoin();
    }

    private GameObject CreateBorderingTileAt(Vector3 hexPosition)
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
                if (go.TryGetComponent<ConcealedTile>(out _))
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
        BorderingTile newTileComponent = newHex.GetComponent<BorderingTile>();

        DefineBiome(position, newTileComponent);
        RandomizeFeatures(newTileComponent);
        DefineIfIsPOI(newTileComponent);
        return newTileComponent.gameObject;
    }

    private void DefineIfIsPOI(BorderingTile newTileComponent)
    {
        Biome biome = newTileComponent.biome;
        if (IsSecondOfBiome(biome))
        {
            newTileComponent.MakePOI();
        }
    }

    private bool IsSecondOfBiome(Biome biome)
    {
        if (borderingTilesCounter.TryGetValue(biome, out int value))
        {
            if (value == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool collidesWithFogTile(Collider[] colliders)
    {
        foreach(var collider in colliders)
        {
            var go = collider.gameObject; //This is the game object you collided with
            if (go.TryGetComponent<ConcealedTile>(out _))
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

    private void DefineBiome(Vector3 hexPosition, BorderingTile newTileComponent)
    {
        // Vector3[] positionsOfHexesAround = HexUtils.PositionsOfHexesAround(hexPosition);
        List<OwnedTile> hexesAround = newTileComponent.GetOwnedHexesAround();
        int randomHexIndex = Utils.GenerateRandomIntNumberWhereMaxIs(6);

        Tile randomTile = hexesAround.ElementAtOrDefault(randomHexIndex - 1);

        Biome biome;
        if (randomTile != null)
        {
            biome = randomTile.biome;
        }
        else
        {
            biome = GetHexBiomeByPosition(hexPosition);
        }

        newTileComponent.biome = biome;
        
        if (borderingTilesCounter.TryGetValue(biome, out int value))
        {
            borderingTilesCounter[biome] += 1;
        }
        else
        {
            borderingTilesCounter.Add(biome, 1);
        }
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

    public GameObject ConvertToOwnedHex(BorderingTile borderingTileComponent)
    {
        Biome biome = borderingTileComponent.biome;
        
        bool hasUniqueResource = borderingTileComponent.hasUniqueResource;
        bool hasSecondaryResource = borderingTileComponent.hasSecondaryResource;
        bool hasTertiaryResource = borderingTileComponent.hasTertiaryResource;
        bool hasRestriction = borderingTileComponent.hasRestriction;

        if (borderingTileComponent.isPOI)
        {
            BuildingBlueprint buildingBlueprint = BuildingQuestsManager.BiomeRewardsDictionary[biome];
            buildingBlueprint.locked = false;
            //TODO define behavior when new Building is unlocked
            // PlayerMessageService.instance.ShowMessage(buildingBlueprint.name + " is unlocked!");
            BuildingQuestsManager.instance.ShowNewBuildingPanel(buildingBlueprint);
        }
        
        //change tilePrefab based on biome
        Vector3 position = borderingTileComponent.gameObject.transform.position;
        Destroy(borderingTileComponent.gameObject);
        GameObject tile = CreateOwnedTile(biome, position);
        SoundManager.PlaySound(SoundManager.Sound.new_tile);
        
        //create features based on biome
        BiomeFeatures biomeFeatures = BiomeFeaturesDictionary[biome];
        if (hasUniqueResource)
        {
            CreateFeature(tile, biomeFeatures.uniqueResource);
        }
        if (hasSecondaryResource)
        {
            CreateFeature(tile, biomeFeatures.secondaryResource);
        }
        if (hasTertiaryResource)
        {
            CreateFeature(tile, biomeFeatures.tertiaryResource);
        }
        //TODO
        /*if (hasRestriction)
        {
            CreateFeature(hex, biomeFeatures.Restriction, biomeFeatures.RestrictionMaxCount);
        }*/
        
        SpawnDecor(tile);

        return tile;
    }

    private void SpawnDecor(GameObject tile)
    {
        int randomCount1 = Utils.GenerateRandomIntBetween(0, 2);
        for (int i = 0; i < randomCount1; i++)
        {
            Vector3 position1 = TileUtils.PositionOnTile(tile.transform.position);
            CreateInTile(rockPrefab, tile, position1);
        }

        int randomCount2 = Utils.GenerateRandomIntBetween(0, 6);
        for (int i = 0; i < randomCount2; i++)
        {
            Vector3 position2 = TileUtils.PositionOnTile(tile.transform.position);
            CreateInTile(stonePrefab, tile, position2);
        }
    }
}
