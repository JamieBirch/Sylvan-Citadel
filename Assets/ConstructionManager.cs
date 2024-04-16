using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;
    public GameObject house;
    public GameObject well;
    public GameObject foodStorage;
    public Vector3 houseOffset;
    
    private float _hexRadius;

    private void Awake()
    {
        instance = this;
        _hexRadius = TerrainManager.HexRadius;
    }

    public void BuildHouse(GameObject hex)
    {
        House houseComponent = house.GetComponent<House>();
        
        if (GameStats.Wood < houseComponent.woodPrice)
        {
            PlayerMessageService.instance.ShowMessage("Not enough wood to build!");
            Debug.Log("Not enough wood to build!");
        }
        else
        {
            var position = PositionOnHex(hex.transform.position)/* + houseOffset*/ /*+ HexUtils.selectOffset*/;
            float houseRotation = Utils.GenerateRandom(0, 360f);
            GameObject newHouse = Instantiate(house, position, Quaternion.AngleAxis(houseRotation, Vector3.up) , hex.transform);

            GameStats.Wood -= houseComponent.woodPrice;

            OwnedHex hexComponent = hex.GetComponent<OwnedHex>();
            hexComponent.AddHouseToHex(newHouse.GetComponent<House>());
        }
    }
    
    public void BuildWell(GameObject hex)
    {
        Well wellComponent = well.GetComponent<Well>();
        
        if (GameStats.Wood < wellComponent.woodPrice)
        {
            PlayerMessageService.instance.ShowMessage("Not enough wood to build!");
            Debug.Log("Not enough wood to build!");
        }
        else
        {
            var position = PositionOnHex(hex.transform.position);
            float houseRotation = Utils.GenerateRandom(0, 360f);
            /*GameObject newWell = */Instantiate(well, position, Quaternion.AngleAxis(houseRotation, Vector3.up) , hex.transform);

            GameStats.Wood -= wellComponent.woodPrice;

            // OwnedHex hexComponent = hex.GetComponent<OwnedHex>();
        }
    }
    
    public void BuildFoodStorage(GameObject hex)
    {
        FoodStorage foodStorageComponent = foodStorage.GetComponent<FoodStorage>();
        
        if (GameStats.Wood < foodStorageComponent.woodPrice)
        {
            PlayerMessageService.instance.ShowMessage("Not enough wood to build!");
            Debug.Log("Not enough wood to build!");
        }
        else
        {
            var position = PositionOnHex(hex.transform.position);
            float buildingRotation = Utils.GenerateRandom(0, 360f);
            /*GameObject newWell = */Instantiate(foodStorage, position, Quaternion.AngleAxis(buildingRotation, Vector3.up) , hex.transform);

            GameStats.Wood -= foodStorageComponent.woodPrice;

            // OwnedHex hexComponent = hex.GetComponent<OwnedHex>();
        }
    }
    
    public Vector3 PositionOnHex(Vector3 hexCenter)
    {
        float minX = hexCenter.x - _hexRadius;
        float maxX = hexCenter.x + _hexRadius;
        
        float minZ = hexCenter.z - _hexRadius;
        float maxZ = hexCenter.z + _hexRadius;

        return new Vector3(Utils.GenerateRandom(minX, maxX), 0f, Utils.GenerateRandom(minZ, maxZ));
    }
    
}
