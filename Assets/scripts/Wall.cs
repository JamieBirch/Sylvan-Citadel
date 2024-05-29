using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject wallPrefab;

    public Dictionary<int, GameObject> walls = new Dictionary<int, GameObject>();

    public void DestroyWall(int key)
    {
        if (walls.ContainsKey(key))
        {
            Destroy(walls[key]);
        }
    }

    private void BuildWall(int key)
    {
        float wallRotation = key * 60;
        GameObject newWall = Instantiate(wallPrefab, transform.position, Quaternion.AngleAxis(wallRotation, Vector3.up), gameObject.transform);
        walls.Add(key, newWall);
    }

    public void SurroundNewTile(OwnedTile newTile)
    {
        List<OwnedTile> ownedHexesAround = newTile.GetOwnedHexesAround();
        List<int> outerSidesIndices = new List<int>();
        outerSidesIndices.Add(0);
        outerSidesIndices.Add(1);
        outerSidesIndices.Add(2);
        outerSidesIndices.Add(3);
        outerSidesIndices.Add(4);
        outerSidesIndices.Add(5);
        ;

        if (ownedHexesAround.Count > 1)
        {
            foreach (OwnedTile ownedTile in ownedHexesAround)
            {
                Vector3 tileRelativePosition = ownedTile.transform.position - newTile.transform.position;
                
                //skip own tile
                if (tileRelativePosition == Vector3.zero)
                {
                    continue;
                }
                
                int otherTileIndex
                    = TileUtils.GetTileIndex(tileRelativePosition);
                outerSidesIndices.Remove(otherTileIndex);
            
                //remove opposite wall of otherTile
                int oppositeSideIndex = GetOppositeSideIndex(otherTileIndex);
                ownedTile.tileWall.DestroyWall(oppositeSideIndex);
            }
        }

        foreach (int outerSidesIndex in outerSidesIndices)
        {
            BuildWall(outerSidesIndex);
        }
    }

    private static int GetOppositeSideIndex(int otherTileIndex)
    {
        if (otherTileIndex >= 3)
        {
            return otherTileIndex - 3;
        }
        else
        {
            return otherTileIndex + 3;
        }
    }
}