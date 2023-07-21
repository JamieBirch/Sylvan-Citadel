using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;
    public GameObject house;
    
    public float HexRadius;
    public Vector3 offsetVector = new Vector3(0, 0, 0);
    public Vector3 firstTileCenter = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }

    public void BuildHouse()
    {
        var position = PositionOnHex(firstTileCenter) + offsetVector;
        Instantiate(house, position, Quaternion.identity);
    }
    
    public Vector3 PositionOnHex(Vector3 hexCenter)
    {
        float minX = hexCenter.x - HexRadius;
        float maxX = hexCenter.x + HexRadius;
        
        float minZ = hexCenter.z - HexRadius;
        float maxZ = hexCenter.z + HexRadius;

        return new Vector3(Utils.GenerateRandom(minX, maxX), 0f, Utils.GenerateRandom(minZ, maxZ));
    }
    
}
