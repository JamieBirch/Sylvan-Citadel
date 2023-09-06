using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public string Name;
    public Biome biome;
    
    public virtual void OnMouseEnter()
    {
        
    }
    
    public List<Hex> GetHexesAround()
    {
        List<Hex> listOfAdjacentHexes = new List<Hex>();
        Vector3 hexPosition = transform.position;

        var overlapColliders = Physics.OverlapSphere(hexPosition, HexUtils.HexSize);
        foreach (Collider _collider in overlapColliders)
        {
            if (_collider.TryGetComponent(out Hex hexComponent))
            {
                listOfAdjacentHexes.Add(hexComponent);
            }
        }

        return listOfAdjacentHexes;
    }
    
    public List<OwnedHex> GetOwnedHexesAround()
    {
        List<OwnedHex> listOfAdjacentOwnedHexes = new List<OwnedHex>();
        Vector3 hexPosition = transform.position;

        var overlapColliders = Physics.OverlapSphere(hexPosition, HexUtils.HexSize);
        foreach (Collider _collider in overlapColliders)
        {
            if (_collider.TryGetComponent(out OwnedHex hexComponent))
            {
                listOfAdjacentOwnedHexes.Add(hexComponent);
                // Debug.Log("Got one!");
            }
        }

        return listOfAdjacentOwnedHexes;
    }
}
