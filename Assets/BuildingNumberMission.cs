using System.Linq;

class BuildingNumberMission : Mission
{
    public Building building;
    public int goalNumber;
    
    public override bool CheckFinished()
    {
        int buildingsCount = GameStats.OwnedTiles.SelectMany(tile => tile.buildings).Where(bd => bd.name == building.name).Count();
        return buildingsCount >= goalNumber;
    }

    public override void GiveWording()
    {
        wording = "Build " + goalNumber + " " + building.name + "s";
    }
}