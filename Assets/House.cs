using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public OwnedHex hex;
    public int woodPrice;
    public int capacity;
    public int bedsAvailable;
    
    private List<Human> _tenants;

    private void Start()
    {
        _tenants = new List<Human>();
        bedsAvailable = capacity;
        hex = GetComponentInParent<OwnedHex>();
    }

    public bool MoveIn(Human human)
    {
        if (bedsAvailable > 0)
        {
            _tenants.Add(human);
            bedsAvailable--;
            hex.BedsAvailable--;
            human.homeHex = hex;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void MoveOut(Human human)
    {
        human.hasHome = false;
        human._home = null;
        _tenants.Remove(human);
        bedsAvailable++;
        hex.BedsAvailable++;
    }
}
