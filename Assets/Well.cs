
public class Well : Building
{
    public override bool IsBuildable()
    {
        return GameStats.Wood >= woodPrice;
    }
}
