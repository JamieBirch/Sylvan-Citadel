
public class Well : Building
{
    public override bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice;
    }
}
