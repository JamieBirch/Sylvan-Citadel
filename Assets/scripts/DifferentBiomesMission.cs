using System.Collections.Generic;
using System.Linq;

class DifferentBiomesMission : Mission
{
    public int goalNumber = 3;

    public override string GiveWording()
    {
       return "Own " + goalNumber + " types of biomes";
    }

    public override bool CheckFinished()
    {
        IEnumerable<Biome> biomes = GameStats.OwnedTiles.Select(tile => tile.biome).Distinct();
        return biomes.Count() >= goalNumber;
    }
}