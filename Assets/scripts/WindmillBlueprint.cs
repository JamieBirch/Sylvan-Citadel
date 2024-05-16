using System.Linq;

public class WindmillBlueprint : BuildingBlueprint
{
    //TODO: TEST
    public int maxWindmillsPerTile = 2;

    public override bool IsBuildable()
    {
        if (TileManager.instance.activeTile != null)
        {
            return GameStats.GetWood() >= woodPrice &&
                   TileManager.instance.GetActiveTileBiome() == Biome.grassland &&
                   TileManager.instance.activeTile.GetComponent<OwnedTile>().buildings
                       .Select(building => building.TryGetComponent<WindmillBlueprint>(out _)).Count() <= maxWindmillsPerTile;
        } else
        {
            return false;
        }
    }

    public override bool IsShowable()
    {
        if (TileManager.instance.activeTile != null && !locked)
        {
            return TileManager.instance.GetActiveTileBiome() == Biome.grassland;
        }
        else
        {
            return false;
        }
    }
}
