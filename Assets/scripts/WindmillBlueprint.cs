using System.Linq;

public class WindmillBlueprint : BuildingBlueprint
{
    // public GameObject propeller;
    // public int produceEvery_seconds;
    public int maxWindmillsPerTile = 2;

    // private float countdown;

    /*private void Start()
    {
        countdown = produceEvery_seconds;
    }

    private void Update()
    {
        //rotate propeller
        propeller.transform.Rotate(0,0, Time.deltaTime * 15);
        
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = produceEvery_seconds;
            GameStats.instance.AddFood();
            Debug.Log("Windmill produced some wheat");
        }
    }*/

    public bool IsBuildable()
    {
        if (TileManager.instance.activeTile != null)
        {
            return base.IsBuildable() &&
                   TileManager.instance.GetActiveTileBiome() == Biome.grassland &&
                   TileManager.instance.activeTile.GetComponent<OwnedTile>().buildings
                       .Select(building => building.TryGetComponent<WindmillBlueprint>(out _)).Count() <= maxWindmillsPerTile;
        } else
        {
            return false;
        }
    }

    public bool IsShowable()
    {
        if (!base.IsShowable())
        {
            return false;
        }
        
        if (TileManager.instance.activeTile != null)
        {
            return TileManager.instance.GetActiveTileBiome() == Biome.grassland;
        }
        else
        {
            return false;
        }
    }
}
