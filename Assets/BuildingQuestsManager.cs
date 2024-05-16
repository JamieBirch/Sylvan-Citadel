using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingQuestsManager : MonoBehaviour
{
    public static BuildingQuestsManager instance;
    
    public static Dictionary<Biome, BuildingBlueprint> BiomeRewardsDictionary = new Dictionary<Biome, BuildingBlueprint>() ;
    public BiomeReward[] BiomeRewards;

    public GameObject newBuildingCanvas;
    public Text headerText;
    public Text descriptionText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        foreach (BiomeReward BiomeReward in BiomeRewards)
        {
            BiomeRewardsDictionary.Add(BiomeReward.biome, BiomeReward.reward);
        }
    }
    
    [System.Serializable]
    public class BiomeReward
    {
        public Biome biome;
        public BuildingBlueprint reward;
    }

    public void ShowNewBuildingPanel(BuildingBlueprint buildingBlueprint)
    {
        Time.timeScale = 0f;

        headerText.text = buildingBlueprint.name + " unlocked!";
        descriptionText.text = buildingBlueprint.description;
        newBuildingCanvas.SetActive(true);
    }
}
