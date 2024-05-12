using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string Name;
    public Biome biome;
    
    public virtual void OnMouseEnter()
    {
        
    }
    
    public List<Tile> GetHexesAround()
    {
        List<Tile> listOfAdjacentHexes = new List<Tile>();
        Vector3 hexPosition = transform.position;

        var overlapColliders = Physics.OverlapSphere(hexPosition, TileUtils.HexSize);
        foreach (Collider _collider in overlapColliders)
        {
            if (_collider.TryGetComponent(out Tile hexComponent))
            {
                listOfAdjacentHexes.Add(hexComponent);
            }
        }

        return listOfAdjacentHexes;
    }
    
    public List<OwnedTile> GetOwnedHexesAround()
    {
        List<OwnedTile> listOfAdjacentOwnedHexes = new List<OwnedTile>();
        Vector3 hexPosition = transform.position;

        var overlapColliders = Physics.OverlapSphere(hexPosition, TileUtils.HexSize);
        foreach (Collider _collider in overlapColliders)
        {
            if (_collider.TryGetComponent(out OwnedTile hexComponent))
            {
                listOfAdjacentOwnedHexes.Add(hexComponent);
                // Debug.Log("Got one!");
            }
        }

        return listOfAdjacentOwnedHexes;
    }
}
