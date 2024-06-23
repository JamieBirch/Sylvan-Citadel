using UnityEngine;

public abstract class Boon
{
    // public string Nickname;
    // public string Description;

    public abstract void ImplementBoon();
    public abstract void RollbackBoon();

    public abstract string GetNickname();

    public abstract string GetDescription();
}

class Founder : Boon
{
    public override void ImplementBoon()
    {
        return;
    }

    public override void RollbackBoon()
    {
        return;
    }

    public override string GetNickname()
    {
        return "The Founder";
    }

    public override string GetDescription()
    {
        return "founder of a new settlement";
    }
}

class Conqueror : Boon
{
    public int conquestDiscount = 1;
    
    public override void ImplementBoon()
    {
        TileManager.instance.currentTilePrice -= conquestDiscount;
    }

    public override void RollbackBoon()
    {
        TileManager.instance.currentTilePrice += conquestDiscount;
    }

    public override string GetNickname()
    {
        return "The Conqueror";
    }

    public override string GetDescription()
    {
        return "all tiles take " + conquestDiscount + " less settler to conquer";
    }
}

class Builder : Boon
{
    public int buildingDiscount = 20;
    
    public override void ImplementBoon()
    {
        ConstructionManager.instance.buildPriceDiscount += buildingDiscount;
    }

    public override void RollbackBoon()
    {
        ConstructionManager.instance.buildPriceDiscount -= buildingDiscount;
    }

    public override string GetNickname()
    {
        return "The Builder";
    }

    public override string GetDescription()
    {
        return "all buildings take " + buildingDiscount + "% less resources to build";
    }
}

public enum BoonType
{
    founder,
    builder,
    conqueror
}

static class BoonLogic
{
    public static Boon GetBoon(this BoonType boonType)
    {
        switch (boonType)
        {
            case BoonType.founder:
                return new Founder();
            case BoonType.builder:
                return new Builder();
            case BoonType.conqueror:
                return new Conqueror();
            default:
                Debug.Log("unknown boon type");
                return new Founder();
        }
    }
}
