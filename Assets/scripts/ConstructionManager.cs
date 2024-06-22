using UnityEngine;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;

    // public GameObject BuildingsButtons;
    public GameObject BuildingPanel;
    public int buildPriceDiscount = 0;

    private void Awake()
    {
        instance = this;
    }

    /*public void ShowHideBuildPanel()
    {
        if (BuildingsButtons.activeSelf)
        {
            BuildingsButtons.SetActive(false);
        }
        else
        {
            BuildingsButtons.SetActive(true);
        }
    }*/
    
    public void ShowBuildPanel()
    {
        BuildingPanel.SetActive(true);
    }
    
    public void HideBuildPanel()
    {
        BuildingPanel.SetActive(false);
    }

    /*public void Update()
    {
        if (TileManager.instance.activeTile == null && BuildButton.IsInteractable())
        {
            BuildButton.interactable = false;
        }
        else if (TileManager.instance.activeTile != null && !BuildButton.IsInteractable())
        {
            BuildButton.interactable = true;
        }
    }*/

    public void Build(BuildingBlueprint buildingBlueprint, GameObject tile)
    {
        int woodPrice = buildingBlueprint.GetComponent<BuildingBlueprint>().buildingWoodPrice;

        if (GameStats.GetWood() < woodPrice)
        {
            PlayerMessageService.instance.ShowMessage("Not enough wood to build!");
            Debug.Log("Not enough wood to build!");
        }
        else
        {
            //instantiate building
            InstantiateBuilding(buildingBlueprint, tile);
            
            //deduct wood
            GameStats.instance.RemoveWood(woodPrice);
        }
    }

    public static void InstantiateBuilding(BuildingBlueprint buildingBlueprint, GameObject tile)
    {
        Vector3 position;
        Vector3 tileCenter = tile.transform.position;
        if (buildingBlueprint.onlyBuildingOnTile)
        {
            position = new Vector3(tileCenter.x, 0f, tileCenter.z);
        }
        else
        {
            position = TileUtils.PositionOnTile(tileCenter);
        }
        
        // float buildingRotation = Utils.GenerateRandom(0, 360f);
        GameObject newBuilding = Instantiate(buildingBlueprint.buildingPrefab, position, 
            Quaternion.identity /*Quaternion.AngleAxis(buildingRotation, Vector3.up)*/, tile.transform);
        Building buildingComponent = newBuilding.GetComponent<Building>();
        buildingComponent.name = buildingBlueprint.name;
        buildingComponent.buildingNameUI.text = buildingBlueprint.name;
        buildingComponent.descriptionUI.text = buildingBlueprint.description;

        //assign to tile
        OwnedTile tileComponent = tile.GetComponent<OwnedTile>();
        tileComponent.AddBuildingToTile(buildingComponent);

        if (buildingBlueprint.onlyBuildingOnTile)
        {
            tileComponent.allowBuildingOnTile = false;
        }

        if (buildingBlueprint.blockTreeGrowth)
        {
            tileComponent.blockTreeGrowth = true;
        }

        if (buildingBlueprint.blocksTerraforming)
        {
            tileComponent.allowTileTerraforming = false;
        }
        
    }
}
