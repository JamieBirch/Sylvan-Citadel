using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PopulationManager _populationManager;
    private TerrainManager _terrainManager;

    public GameObject GameOverCanvas;
    public GameObject InfoCanvas;
    public List<Mission> missions;
    
    public int StartHumans;
    
    //start resources
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
        
        //create terrain by creating tiles 
        startHex = _terrainManager.CreateStartTile();
        //create other tiles
        _terrainManager.CreateConcealedHexesAround(startHex);

        // put storage resources to storage
        GameStats.instance.AddFood(StartStorageFood);
        GameStats.instance.AddWood(StartStorageWood);

        // spawn humans
        _populationManager.SpawnHumans(StartHumans, startHex);
        
        SoundManager.PlaySoundTrack();
    }

    // Update is called once per frame
    void Update()
    {
        humans.text = GameStats.GetPopulation().ToString();
        food.text = GameStats.GetFood().ToString();
        wood.text = GameStats.GetWood().ToString();
        tiles.text = GameStats.OwnedTiles.Count.ToString();
        
        if (GameStats.GetPopulation() <= 0)
        {
            PlayerMessageService.instance.ShowMessage("No population left!");
            Debug.Log("No population left!");
        }

        if (CheckAllMissionsComplete())
        {
            Debug.Log("All Missions complete, game over");
            GameOver();
        }
    }

    private void GameOver()
    {
        InfoCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
    }


    private bool CheckAllMissionsComplete()
    {
        if (missions.Count > 0)
        {
            return missions.All(mission => mission.finished);
        }
        else
        {
            return false;
        }
    }

}