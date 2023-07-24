using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager instance;
    public GameObject house;
    private float _hexRadius;
    
    // public float HexRadius;
    public Vector3 offsetVector = new Vector3(0, 0, 0);
    public Vector3 firstTileCenter = Vector3.zero;

    private void Awake()
    {
        instance = this;
        _hexRadius = TerrainManager.HexRadius;
    }

    public void BuildHouse()
    {
        var position = PositionOnHex(firstTileCenter) + offsetVector;
        Instantiate(house, position, Quaternion.identity);
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
