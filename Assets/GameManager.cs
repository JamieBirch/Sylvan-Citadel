using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PopulationManager _populationManager;
    private TerrainManager _terrainManager;
    private ConstructionManager _constructionManager;
    
    public GameObject sun;
    
    public int StartHumans;
    
    //initial resources
    public int StartStorageWood;
    public int StartStorageFood;

    public GameObject startHex;

    public static int day;
    public float countdown;
    public float dayLength = 60f;

    public Text daysText;
    public Text countdownText;
    public Text fruits;
    public Text humans;
    public Text food;
    public Text wood;

    public static event Action NewDay;

    // Start is called before the first frame update
    void Start()
    {
        _populationManager = PopulationManager.instance;
        _terrainManager = TerrainManager.instance;
        _constructionManager = ConstructionManager.instance;
        
        GameStats.Population = 0;

        //create terrain by creating hexes 
        startHex = _terrainManager.CreateStartHex();
        //create other hexes
        _terrainManager.CreateConcealedHexes();

        // put storage resources to storage
        GameStats.Food = StartStorageFood;
        GameStats.Wood = StartStorageWood;

        // spawn humans
        _populationManager.SpawnHumans(StartHumans, startHex);

        // start world time
        countdown = dayLength;
        day = 1;
        
        NewDay?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        fruits.text = GameStats.FruitsAvailable.ToString();
        humans.text = GameStats.Population.ToString();
        food.text = GameStats.Food.ToString();
        wood.text = GameStats.Wood.ToString();
        
        var sunTransform = sun.transform;

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        sunTransform.Rotate(Time.deltaTime * (360/dayLength),0, 0);
        
        if (countdown <= 0)
        {
            //start new day
            day++;
            countdown = dayLength;
            sunTransform.rotation = Quaternion.Euler(0, -60, 0);
            NewDay?.Invoke();
        }

        countdownText.text = string.Format("{0:00.00}", countdown);
        daysText.text = day.ToString();
        
    }

    public void BuildHouse()
    {
        _constructionManager.BuildHouse(startHex);
    }

}
