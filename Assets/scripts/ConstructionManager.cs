using UnityEngine;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;

    public Button BuildButton;
    public GameObject BuildingsButtons;
    
    private void Awake()
    {
        instance = this;
    }

    public void ShowHideBuildPanel()
    {
        if (BuildingsButtons.activeSelf)
        {
            BuildingsButtons.SetActive(false);
        }
        else
        {
            BuildingsButtons.SetActive(true);
        }
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
        int woodPrice = buildingBlueprint.GetComponent<BuildingBlueprint>().woodPrice;

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
        var position = TileUtils.PositionOnTile(tile.transform.position);
        float buildingRotation = Utils.GenerateRandom(0, 360f);
        GameObject newBuilding = Instantiate(buildingBlueprint.buildingPrefab, position,
            Quaternion.AngleAxis(buildingRotation, Vector3.up), tile.transform);
        Building buildingComponent = newBuilding.GetComponent<Building>();
        buildingComponent.name = buildingBlueprint.name;

        //assign to tile
        OwnedTile tileComponent = tile.GetComponent<OwnedTile>();
        tileComponent.AddBuildingToTile(buildingComponent);
    }
}
