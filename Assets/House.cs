using System.Collections.Generic;

public class House : Building
{
    public OwnedHex hex;
    public int capacity;
    private int bedsAvailable;
    
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
            // hex.BedsAvailable--;
            // human.homeHex = hex;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetBedsAvailable()
    {
        return bedsAvailable;
    }
    
    public void MoveOut(Human human)
    {
        _tenants.Remove(human);
        bedsAvailable++;
        // hex.BedsAvailable++;
    }

    public override bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice;
    }
}
