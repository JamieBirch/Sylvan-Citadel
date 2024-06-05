using UnityEngine;

public class ForestBlueprint : BuildingBlueprint
{
    public override bool IsBuildable()
    {
        if (TileManager.instance.activeTile != null)
        {
            return GameStats.GetWood() >= woodPrice &&
                   //only building per tile
                   TileManager.instance.activeTile.GetComponent<OwnedTile>().buildings.Count == 0;
        } else
        {
            return false;
        }
    }

    public override bool IsShowable()
    {
        if (TileManager.instance.activeTile != null && !locked)
        {
            return TileManager.instance.GetActiveTileBiome() == Biome.forest &&
                   TileManager.instance.activeTile.GetComponent<OwnedTile>().GetWoodland() != null;
        }
        else
        {
            return false;
        }
    }
}
