using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingQuestsManager : MonoBehaviour
{
    public static BuildingQuestsManager instance;
    
    public static Dictionary<BuildingUnlockTrigger, BuildingBlueprint> TriggerRewardsDictionary = new Dictionary<BuildingUnlockTrigger, BuildingBlueprint>() ;
    public TriggerReward[] TriggerRewards;

    public GameObject newBuildingCanvas;
    public Text headerText;
    public Text descriptionText;

    public bool DeadFruitTreeBuildingUnlocked = false;
    public bool DeadPineTreeBuildingUnlocked = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        foreach (TriggerReward TriggerReward in TriggerRewards)
        {
            TriggerRewardsDictionary.Add(TriggerReward.trigger, TriggerReward.reward);
        }
    }
    
    [System.Serializable]
    public class TriggerReward
    {
        public BuildingUnlockTrigger trigger;
        public BuildingBlueprint reward;
    }

    public enum BuildingUnlockTrigger
    {
        well,
        storage,
        windmill,
        orchard,
        forester
    }
    
    public void UnlockNewBuilding(BuildingUnlockTrigger trigger)
    {
        BuildingBlueprint buildingBlueprint = TriggerRewardsDictionary[trigger];
        buildingBlueprint.locked = false;
        
        Time.timeScale = 0f;

        headerText.text = buildingBlueprint.name + " unlocked!";
        descriptionText.text = buildingBlueprint.description;
        newBuildingCanvas.SetActive(true);
    }
}
