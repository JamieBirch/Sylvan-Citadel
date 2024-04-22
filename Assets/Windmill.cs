using UnityEngine;

public class Windmill : Building
{
    public GameObject propeller;
    public int produceEvery_seconds;
    // public int windmillDayLength;

    //per day
    // public int foodProduction;
    
    // private float blah = Calendar.dayLength / foodProduction;
    
    private float countdown;

    private void Start()
    {
        countdown = produceEvery_seconds;
    }

    private void Update()
    {
        //turn propeller
        propeller.transform.Rotate(0,0, Time.deltaTime * 15);
        // float blah = Calendar.dayLength / foodProduction;
        
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = produceEvery_seconds;
            GameStats.instance.AddFood();
            Debug.Log("Windmill produced some wheat");
        }
    }

    public override bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice &&
               TileManager.instance.GetActiveTileBiome() == Biome.grassland;
    }

    public override bool IsShowable()
    {
        return TileManager.instance.GetActiveTileBiome() == Biome.grassland;
    }
}
