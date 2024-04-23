
public class Well : Building
{
    public override bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice;
    }

    public override bool IsShowable()
    {
        return true;
    }
}
