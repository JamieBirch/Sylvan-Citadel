
public class FoodStorage : Building
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
