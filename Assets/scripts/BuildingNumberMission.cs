using System.Linq;
using UnityEngine.Serialization;

class BuildingNumberMission : Mission
{
    [FormerlySerializedAs("building")] public BuildingBlueprint buildingBlueprint;
    public int goalNumber;
    
    public override bool CheckFinished()
    {
        int buildingsCount = GameStats.OwnedTiles.SelectMany(tile => tile.buildings).Where(bd => bd.name == buildingBlueprint.name).Count();
        return buildingsCount >= goalNumber;
    }

    public override void GiveWording()
    {
        wording = "Build " + goalNumber + " " + buildingBlueprint.name + "s";
    }
}