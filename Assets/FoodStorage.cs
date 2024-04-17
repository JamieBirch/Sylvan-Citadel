
public class FoodStorage : Building
{
    public override bool IsBuildable()
    {
        return GameStats.Wood >= woodPrice;
    }
}
