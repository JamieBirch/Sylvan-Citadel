using UnityEngine;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;

    public Button BuildButton;
    public GameObject BuildingsButtons;
    
    public GameObject house;
    public GameObject well;
    public GameObject foodStorage;
    
    private float _hexRadius;

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

    public void Update()
    {
        if (TileManager.instance.activeTile == null && BuildButton.IsInteractable())
        {
            BuildButton.interactable = false;
        }
        else if (TileManager.instance.activeTile != null && !BuildButton.IsInteractable())
        {
            BuildButton.interactable = true;
        }
    }

    public void Build(GameObject buildingPrefab, GameObject tile)
    {
        int woodPrice = buildingPrefab.GetComponent<Building>().woodPrice;

        if (GameStats.Wood < woodPrice)
        {
            PlayerMessageService.instance.ShowMessage("Not enough wood to build!");
            Debug.Log("Not enough wood to build!");
        }
        else
        {
            //instantiate building
            var position = TileUtils.PositionOnTile(tile.transform.position);
            float buildingRotation = Utils.GenerateRandom(0, 360f);
            GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.AngleAxis(buildingRotation, Vector3.up) , tile.transform);
            
            //assign to tile
            OwnedHex tileComponent = tile.GetComponent<OwnedHex>();
            tileComponent.AddBuildingToTile(newBuilding.GetComponent<Building>());
            
            //deduct wood
            GameStats.Wood -= woodPrice;
        }
    }
    
}
