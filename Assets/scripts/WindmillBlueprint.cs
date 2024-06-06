using System.Linq;

public class WindmillBlueprint : BuildingBlueprint
{
    public int maxWindmillsPerTile;

    public override bool IsBuildable()
    {
        if (base.IsBuildable())
        {
            return TileManager.instance.activeTile.GetComponent<OwnedTile>().buildings
                .FindAll(building => building.TryGetComponent<Windmill>(out _)).Count() < maxWindmillsPerTile;
        }
        else
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
