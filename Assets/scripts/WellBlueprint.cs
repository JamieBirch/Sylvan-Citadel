
public class WellBlueprint : BuildingBlueprint
{
    public override bool IsBuildable()
    {
        return TileManager.instance.activeTile != null && GameStats.GetWood() >= woodPrice;
    }

    public override bool IsShowable()
    {
        return !locked;
    }
}
