using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PopulationManager _populationManager;
    private TerrainManager _terrainManager;
    
    public GameObject sun;
    
    public int StartHumans;
    
    //initial resources
    public int StartStorageWood;
    public int StartStorageFood;

    public Vector3 firstTileCenter = Vector3.zero;

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
        
        GameStats.Population = 0;

        //create terrain by creating hexes 
        _terrainManager.CreateTerrain();

        // put storage resources to storage
        GameStats.Food = StartStorageFood;
        GameStats.Wood = StartStorageWood;

        // spawn humans
        // move to hex
        _populationManager.SpawnHumans(StartHumans, firstTileCenter);

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

}
