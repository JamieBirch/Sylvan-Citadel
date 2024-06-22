using System.Linq;
using UnityEngine.Serialization;

class BuildingNumberMission : Mission
{
    [FormerlySerializedAs("building")] public BuildingBlueprint buildingBlueprint;
    public int goalNumber = 5;
    
    public override bool CheckFinished()
    {
        int buildingsCount = GameStats.OwnedTiles.SelectMany(tile => tile.buildings).Where(bd => bd.Name == buildingBlueprint.name).Count();
        return buildingsCount >= goalNumber;
    }

    public override string GiveWording()
    {
        return "Build " + goalNumber + " " + buildingBlueprint.name + "s";
    }
}