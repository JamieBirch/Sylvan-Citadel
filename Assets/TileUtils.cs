using UnityEngine;

public static class TileUtils
{
    public static int HexSize = 9;
    
    // public static Vector3 xHexOffset = new Vector3(9f, 0f, 0f);
    // public static Vector3 zHexOffset = new Vector3(0f, 0f, 7.75f);
    
    public static Vector3 xHexOffset = new Vector3(8.75f, 0f, 0f);
    public static Vector3 zHexOffset = new Vector3(0f, 0f, 7.625f);
    
    public static Vector3 selectOffset = new Vector3(0f, 1f, 0f);
    
    // public static float HexRadius = 3f;
    public static float TileRadius = 3f;


    public static Vector3[] PositionsOfHexesAround(Vector3 hexPosition)
    {
        Vector3 rightHexPosition = hexPosition + xHexOffset;
        Vector3 leftHexPosition = hexPosition + -xHexOffset;
        Vector3 topRightHexPosition = hexPosition + xHexOffset/2 + zHexOffset;
        Vector3 topLeftHexPosition = hexPosition + -xHexOffset/2 + zHexOffset;
        Vector3 bottomRightHexPosition = hexPosition + xHexOffset/2 - zHexOffset;
        Vector3 bottomLeftHexPosition = hexPosition + -xHexOffset/2 - zHexOffset;

        return new[]
        {
            rightHexPosition, leftHexPosition, topRightHexPosition, topLeftHexPosition, bottomRightHexPosition,
            bottomLeftHexPosition
        };
    }
    
    public static Vector3 PositionOnTile(Vector3 hexCenter)
    {
        float minX = hexCenter.x - TileRadius;
        float maxX = hexCenter.x + TileRadius;
        
        float minZ = hexCenter.z - TileRadius;
        float maxZ = hexCenter.z + TileRadius;

        return new Vector3(Utils.GenerateRandom(minX, maxX), 0f, Utils.GenerateRandom(minZ, maxZ));
    }
}
