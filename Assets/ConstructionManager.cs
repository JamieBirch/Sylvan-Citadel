using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;
    public GameObject house;
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
            Debug.Log("Not enough wood to build!");
            return;
        }
        else
        {
            var position = PositionOnHex(hex.transform.position) + houseOffset;
            float houseRotation = Utils.GenerateRandom(0, 360f);
            GameObject newHouse = Instantiate(house, position, Quaternion.AngleAxis(houseRotation, Vector3.up) , hex.transform);

            GameStats.Wood -= houseComponent.woodPrice;

            OwnedHex hexComponent = hex.GetComponent<OwnedHex>();
            hexComponent.buildings.Add(newHouse);
            hexComponent.BedsAvailable += houseComponent.capacity;
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
