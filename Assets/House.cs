using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int woodPrice;
    public int capacity;
    public int bedsAvailable;
    
    private List<Human> _tenants;

    private void Start()
    {
        _tenants = new List<Human>();
        bedsAvailable = capacity;
        GameStats.BedsAvailable += capacity;
    }

    public bool MoveIn(Human human)
    {
        if (bedsAvailable > 0)
        {
            _tenants.Add(human);
            bedsAvailable--;
            GameStats.BedsAvailable--;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void MoveOut(Human human)
    {
        _tenants.Remove(human);
        bedsAvailable++;
        GameStats.BedsAvailable++;
    }
}
