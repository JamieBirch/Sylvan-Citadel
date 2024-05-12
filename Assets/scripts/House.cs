using System.Collections.Generic;
using UnityEngine.Serialization;

public class House : Building
{
    [FormerlySerializedAs("hex")] public OwnedTile tile;
    public int capacity;
    private int bedsAvailable;
    
    private List<Human> _tenants;

    private void Start()
    {
        _tenants = new List<Human>();
        bedsAvailable = capacity;
        tile = GetComponentInParent<OwnedTile>();
    }

    public bool MoveIn(Human human)
    {
        if (bedsAvailable > 0)
        {
            _tenants.Add(human);
            bedsAvailable--;
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
    }

    public override bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice;
    }

    public override bool IsShowable()
    {
        return true;
    }
}
