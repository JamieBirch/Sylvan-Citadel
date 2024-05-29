using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public static class TileUtils
{
    public static int HexSize = 9;
    
    public static Vector3 xHexOffset = new Vector3(8.75f, 0f, 0f);
    public static Vector3 zHexOffset = new Vector3(0f, 0f, 7.625f);
    
    // public static Vector3 selectOffset = new Vector3(0f, 1f, 0f);
    
    public static float TileRadius = 3f;


    public static Vector3[] PositionsOfTilesAround(Vector3 hexPosition)
    {
        Vector3 topRightHexPosition = hexPosition + xHexOffset/2 + zHexOffset;
        Vector3 rightHexPosition = hexPosition + xHexOffset;
        Vector3 bottomRightHexPosition = hexPosition + xHexOffset/2 - zHexOffset;
        Vector3 bottomLeftHexPosition = hexPosition + -xHexOffset/2 - zHexOffset;
        Vector3 leftHexPosition = hexPosition + -xHexOffset;
        Vector3 topLeftHexPosition = hexPosition + -xHexOffset/2 + zHexOffset;

        return new[]
        {
            topRightHexPosition, 
            rightHexPosition, 
            bottomRightHexPosition,
            bottomLeftHexPosition,
            leftHexPosition, 
            topLeftHexPosition, 
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

    public static int GetTileIndex(Vector3 tileRelativePosition)
    {
        Vector3[] positionsOfTilesAround = PositionsOfTilesAround(Vector3.zero);

        for (int i = 0; i < positionsOfTilesAround.Length; i++)
        {
            if (positionsOfTilesAround[i] == tileRelativePosition)
            {
                return i;
            }
        }

        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAA");
        return 6;
    }
}
