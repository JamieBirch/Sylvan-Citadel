using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private PopulationManager _populationManager;
    private TerrainManager _terrainManager;

    public GameObject GameOverCanvas;
    public GameObject InfoCanvas;
    public GameObject PauseMenuCanvas;
    public GameObject MissionsCompleteCanvas;
    public Text MissionsCompleteMonarchLLText;
    public Text MissionsCompleteButtonText;
    
    public GameObject ChooseNewMonarchCanvas;
    public GameObject monarchInfoContainer1;
    public GameObject monarchInfoContainer2;
    public GameObject monarchInfoPrefab;

    public Monarch currentMonarch;
    public MonarchPanel MonarchPanel;
    [FormerlySerializedAs("missions")] public List<MissionPrefab> missionObjects;
    public int missionsNumber = 2;
    
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

    public bool waitingForPlayer = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _populationManager = PopulationManager.instance;
        _terrainManager = TerrainManager.instance;
        
        //create start monarch
        AppointNewMonarch(CreateStartMonarch());
        
        //create terrain by creating tiles 
        startHex = _terrainManager.CreateStartTile();
        //create other tiles
        _terrainManager.CreateConcealedTilesAround(startHex);

        // put storage resources to storage
        GameStats.instance.AddFood(StartStorageFood);
        GameStats.instance.AddWood(StartStorageWood);

        // spawn humans
        _populationManager.SpawnHumans(StartHumans, startHex);
        
        SoundManager.PlaySoundTrack();
    }

    private Monarch CreateStartMonarch()
    {
        // monarch.Name = NameGenerator.CreateHumanName();
        // monarch.boon = new Founder();
        List<Mission> monarchMissions = new List<Mission>();
        monarchMissions.Add(new DifferentBiomesMission(2));
        // monarchMissions.Add(new ReachPopulationCountMission(20));
        // monarchMissions.Add(new CollectResourceMission(20, CollectableResource.food));
        // monarch.missions = monarchMissions;
        Monarch monarch = new Monarch(NameGenerator.CreateHumanName(), new Founder(), monarchMissions);

        return monarch;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            PauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        
        if (Input.GetKey("space"))
        {
            if (Calendar.instance.paused)
            {
                Calendar.instance.Resume();
            }
            else
            {
                // Calendar.instance.Pause();
                Calendar.instance.paused = true;
                Time.timeScale = 0f;
            }
        }
        
        humans.text = GameStats.GetPopulation().ToString();
        food.text = GameStats.GetFood().ToString();
        wood.text = GameStats.GetWood().ToString();
        tiles.text = GameStats.OwnedTiles.Count.ToString();
        
        if (GameStats.GetPopulation() <= 0)
        {
            PlayerMessageService.instance.ShowMessage("No population left!");
            Debug.Log("No population left!");
        }

        if (CheckAllMissionsComplete() && !waitingForPlayer)
        {
            // Debug.Log("All Missions complete, game over");
            waitingForPlayer = true;
            // GameOver();

            ShowMissionsCompletePanel();
        }
    }

    private void ShowMissionsCompletePanel()
    {
        MissionsCompleteCanvas.SetActive(true);
        MissionsCompleteMonarchLLText.text = "Long live " + currentMonarch.GetNamePlusNickname() + "!";
        MissionsCompleteButtonText.text = "continue as " + currentMonarch.Name;
        
        Time.timeScale = 0f;
    }

    public void ContinueAsCurrentMonarch()
    {
        if (MissionsCompleteCanvas)
        {
            MissionsCompleteCanvas.SetActive(false);
        }

        waitingForPlayer = false;
        Calendar.instance.Resume();
    }

    public void ChooseNewMonarchPanel()
    {
        ChooseNewMonarchCanvas.SetActive(true);
        // Boon boon = Utils.RandomEnumValue<BoonType>().GetBoon();

        Monarch monarch1 = GenerateMonarch(new Builder());
        Monarch monarch2 = GenerateMonarch(new Conqueror());

        //destroy previous candidates info
        foreach(Transform child in monarchInfoContainer1.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in monarchInfoContainer2.transform)
        {
            Destroy(child.gameObject);
        }
        
        //set new candidates
        Instantiate(monarchInfoPrefab, monarchInfoContainer1.transform).GetComponent<CandidateInfo>().SetStuff(monarch1);
        Instantiate(monarchInfoPrefab, monarchInfoContainer2.transform).GetComponent<CandidateInfo>().SetStuff(monarch2);
    }

    private Monarch GenerateMonarch(Boon boon)
    {
        List<Mission> monarchMissions = new List<Mission>();
        MissionGeneration _missionGeneration = MissionGeneration.instance;
        for (int i = 0; i < missionsNumber; i++)
        {
            Mission generatedMission = _missionGeneration.GenerateMission();
            monarchMissions.Add(generatedMission);
        }
        return new Monarch(NameGenerator.CreateHumanName(), boon, monarchMissions);
    }

    public void ChooseMonarch(Monarch monarch)
    {
        ChooseNewMonarchCanvas.SetActive(false);
        currentMonarch.boon.RollbackBoon();
        AppointNewMonarch(monarch);
        waitingForPlayer = false;
        Calendar.instance.Resume();
    }

    private void AppointNewMonarch(Monarch monarch)
    {
        currentMonarch = monarch;
        currentMonarch.boon.ImplementBoon();
        UpdateMonarchPanel();
    }

    private void UpdateMonarchPanel()
    {
        missionObjects = MonarchPanel.SetNewMonarch(currentMonarch);
    }

    private void GameOver()
    {
        InfoCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        
        Time.timeScale = 0f;
    }


    private bool CheckAllMissionsComplete()
    {
        // List<Mission> missions = currentMonarch.missions;
        if (missionObjects.Count > 0)
        {
            return missionObjects.All(missionO => missionO.mission.finished);
        }
        else
        {
            return false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}
