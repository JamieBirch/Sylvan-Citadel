using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OwnedTile : Tile
{
    public const string PopulationStatString = "population";
    public const string BedsAvailableStatString = "beds";
    private TileManager _tileManager;
    private TerrainManager _terrainManager;
    
    public GameObject tileBase;

    public GameObject groveLayers;
    public GameObject forestLayers;
    public GameObject grasslandLayers;
    
    // public GameObject groveTile;
    // public GameObject forestTile;
    // public GameObject grasslandTile;
    public GameObject riverTile;
    public GameObject mountainTile;
    public GameObject swampTile;
    
    private Color defaultColor = new Color(1f, 1f, 1f, 0f);
    private Color selectColor = new Color(1f, 1f, 1f, 0.7f);
    
    // public Vector3 selectOffset;

    public Renderer rend;

    public GameObject tileStatsUIprefab;
    public TileStatsUI tileStatsUI;
    public GameObject tileBuildingsUI;

    public Dictionary<LandscapeFeatureType, LandscapeFeature> LandscapeFeaturesDictionary = new Dictionary<LandscapeFeatureType, LandscapeFeature>();
    private Dictionary<string, int> tileStatistics = new Dictionary<string, int>();
    
    public GameObject village;
    public List<Building> buildings = new List<Building>();
    public bool allowBuildingOnTile = true;
    public bool allowTileTerraforming = true;
    public bool blockTreeGrowth = false;
    private bool selected = false;

    public Wall tileWall;

    //Stats
    private int BedsAvailable;
    public int HexPopulation;

    // public GameObject settlersAvailableCanvas;
    public Text settlersAvailableText;
    private int settlersAvailable;

    public void UpdateTileStatistics(string field, int count)
    {
        tileStatistics[field] = count;
    }
    
    public void UpdateTileStatisticsUI(string field)
    {
        tileStatsUI.UpdateFieldUi(field, tileStatistics[field]);
    }
    
    private void Start()
    {
        _tileManager = TileManager.instance;
        _terrainManager = TerrainManager.instance;
        
        Calendar.NewDay += StartDay;

        BedsAvailable = 0;
        
        string[] defaultTileStatFields =  {
            PopulationStatString,
            BedsAvailableStatString
        };
        
        tileStatsUI.TileName.text = Name;

        foreach (string field in defaultTileStatFields)
        {
            tileStatistics.Add(field, 0);
            tileStatsUI.AddStatField(field, 0);
        }
        foreach (LandscapeFeature feature in LandscapeFeaturesDictionary.Values)
        {
            LandscapeFeatureType landscapeFeatureType = feature.getFeatureType();
            int count = feature.getCount();
            tileStatistics.Add(landscapeFeatureType.ToString(), count);
            tileStatsUI.AddStatField(landscapeFeatureType.ToString(), count);
        }
    }
    
    void StartDay()
    {
        LandscapeFeatureWoodland woodland = GetWoodland();
        if (woodland != null)
        {
            woodland.GrowNewTree(_terrainManager);
        }
    }

    public void Update()
    {
        settlersAvailable = HexPopulation;
        
        if (buildings.Count > 0)
        {
            BedsAvailable = CalcBedsAvailableSum();
            // Debug.Log("total beds: " + BedsAvailable);
            
            tileBuildingsUI.SetActive(true);
        }
        else
        {
            tileBuildingsUI.SetActive(false);
        }

        UpdateTileStatistics(PopulationStatString, HexPopulation);
        UpdateTileStatistics(BedsAvailableStatString, BedsAvailable);
        
        foreach (LandscapeFeature feature in LandscapeFeaturesDictionary.Values)
        {
            LandscapeFeatureType landscapeFeatureType = feature.getFeatureType();
            int count = feature.getCount();
            UpdateTileStatistics(landscapeFeatureType.ToString(), count);
        }
    }

    public int GetBedsAvailable()
    {
        return BedsAvailable;
    }

    public void AddBuildingToTile(Building buildingComponent)
    {
        buildings.Add(buildingComponent);
        buildingComponent.tile = this;
        tileStatsUI.AddBuildingField(buildingComponent.name);
    }
    
    public void RemoveBuildingFromTile(Building buildingComponent)
    {
        buildings.Remove(buildingComponent);
        tileStatsUI.RemoveBuildingField(buildingComponent.name);
        flagsToDefault();
    }

    private void flagsToDefault()
    {
        if (!allowBuildingOnTile)
        {
            allowBuildingOnTile = true;
        }

        if (blockTreeGrowth)
        {
            blockTreeGrowth = false;
        }

        if (!allowTileTerraforming)
        {
            allowTileTerraforming = true;
        }
    }

    private int CalcBedsAvailableSum()
    {
        int bedsAvailableSum = 0;
        foreach (Building building in buildings)
        {
            // House component;
            if (building.gameObject.TryGetComponent<House>(out House houseComponent))
            {
                bedsAvailableSum += houseComponent.GetBedsAvailable();
            }
            // Debug.Log("adding beds: " + house.GetBedsAvailable() + "current sum: " + BedsAvailable);
        }
        return bedsAvailableSum;
    }

    private int CalculateSettlersAvailable()
    {
        return HexPopulation / 2;
    }

    public int GetSettlersAvailable()
    {
        return settlersAvailable;
    }

    public void SettleInHex(Human human)
    {
        // Debug.Log("settling " + human.Name + " in " + Name);
        if (human.homeTile != this)
        {
            // Debug.Log("new Hex!");
            _tileManager.RelocateHumanTo(this, village.GetComponent<Village>(), human);
        }
    }

    public override void OnMouseEnter()
    {
        highlight();
        _tileManager.ShowTileStats(this);
    }

    private void highlight()
    {
        rend.materials[1].color = selectColor;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            if (!selected)
            {
                // gameObject.transform.position += HexUtils.selectOffset;
                Select();
            }
            else
            {
                Unselect();
            }
        }
    }

    private void Select()
    {
        selected = true;
        _tileManager.SetTileAsActive(gameObject);
        highlight();
        SoundManager.PlaySound(SoundManager.Sound.tile_select);
        
        //TODO show Build & Terraform panels
        _tileManager.ShowBuildAndTerraformPanels();
    }

    private void OnMouseExit()
    {
        if (!selected)
        {
            ColorToDefault();
            _tileManager.HideTileStats(this);
        }
    }

    public void Unselect()
    {
        selected = false;
        _tileManager.SetHexAsInActive();
        ColorToDefault();
        
        //TODO hide Build & Terraform panels
        _tileManager.HideBuildAndTerraformPanels();
    }

    private void ColorToDefault()
    {
        rend.materials[1].color = defaultColor;
    }

    public bool NeighborsHaveAvailableBeds()
    {
        // Debug.Log("checking for available beds in nearby hexes");
        List<OwnedTile> ownedHexesAround = GetOwnedHexesAround();
        foreach (var ownedHex in ownedHexesAround)
        {
            if (ownedHex.BedsAvailable > 0)
            {
                // Debug.Log("there are available beds in nearby hexes");
                return true;
            } 
        }
        return false;
    }

    public LandscapeFeatureWoodland GetWoodland()
    {
        
        if (LandscapeFeaturesDictionary.ContainsKey(LandscapeFeatureType.fruitTrees))
        {
            return (LandscapeFeatureWoodland)LandscapeFeaturesDictionary[LandscapeFeatureType.fruitTrees];
        }
        else if (LandscapeFeaturesDictionary.ContainsKey(LandscapeFeatureType.pineTrees))
        {
            return (LandscapeFeatureWoodland)LandscapeFeaturesDictionary[LandscapeFeatureType.pineTrees];
        }
        else
        {
            // Debug.Log("No woodland in tile " + name);
            return null;
        }
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}