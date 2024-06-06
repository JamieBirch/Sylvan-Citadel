
public class FoodStorageBlueprint : BuildingBlueprint
{
    /*public override bool IsBuildable()
    {
        return base.IsBuildable();
    }*/
    
    public override bool IsShowable()
    {
        return !locked;
    }
}
