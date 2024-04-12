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
        
        GameStats.Population = 0;

        //create terrain by creating hexes 
        startHex = _terrainManager.CreateStartHex();
        GameStats.OwnedHexes++;
        //create other hexes
        _terrainManager.CreateConcealedHexesAround(startHex);

        // put storage resources to storage
        GameStats.Food = StartStorageFood;
        GameStats.Wood = StartStorageWood;

        // spawn humans
        _populationManager.SpawnHumans(StartHumans, startHex);
    }

    // Update is called once per frame
    void Update()
    {
        // fruits.text = GameStats.FruitsAvailable.ToString();
        humans.text = GameStats.Population.ToString();
        food.text = GameStats.Food.ToString();
        wood.text = GameStats.Wood.ToString();
        tiles.text = GameStats.OwnedHexes.ToString();
        
        if (GameStats.Population <= 0)
        {
            PlayerMessageService.instance.ShowMessage("No population left!");
            Debug.Log("No population left!");
        }
    }

}
