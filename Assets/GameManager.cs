using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PopulationManager _populationManager;
    private TerrainManager _terrainManager;
    
    public int StartHumans;
    
    //initial resources
    public int StartStorageWood;
    public int StartStorageFood;

    public GameObject startHex;

    // public Text fruits;
    public Text humans;
    public Text food;
    public Text wood;
    public Text tiles;

    void Start()
    {
        _populationManager = PopulationManager.instance;
        _terrainManager = TerrainManager.instance;
        
        // GameStats.Population = 0;

        //create terrain by creating hexes 
        startHex = _terrainManager.CreateStartHex();
        //create other hexes
        _terrainManager.CreateConcealedHexesAround(startHex);

        // put storage resources to storage
        GameStats.instance.AddFood(StartStorageFood);
        GameStats.instance.AddWood(StartStorageWood);

        // spawn humans
        _populationManager.SpawnHumans(StartHumans, startHex);
    }

    // Update is called once per frame
    void Update()
    {
        // fruits.text = GameStats.FruitsAvailable.ToString();
        humans.text = GameStats.GetPopulation().ToString();
        food.text = GameStats.GetFood().ToString();
        wood.text = GameStats.GetWood().ToString();
        tiles.text = GameStats.OwnedTiles.Count.ToString();
        
        if (GameStats.GetPopulation() <= 0)
        {
            PlayerMessageService.instance.ShowMessage("No population left!");
            Debug.Log("No population left!");
        }
    }

}
