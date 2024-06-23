using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MissionGeneration : MonoBehaviour
{
    public static MissionGeneration instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        /*foreach (SoundAudioClip soundAudioClip in soundAudioClips)
        {
            soundsDictionary.Add(soundAudioClip.sound, soundAudioClip.audioClip);
        }*/
    }


    public Mission GenerateMission()
    {
        //TODO test
        return Utils.RandomEnumValue<MissionType>().GetMission();
    }
}

public enum MissionType
{
    collect_resource,
    reach_population,
    conquer_biomes
}

static class MissionLogic
{
    public static Mission GetMission(this MissionType missionType)
    {
        switch (missionType)
        {
            case MissionType.collect_resource:
                return new CollectResourceMission(/*100, Utils.RandomEnumValue<CollectableResource>()*/);
            case MissionType.reach_population:
                return new ReachPopulationCountMission();
            case MissionType.conquer_biomes:
                return new DifferentBiomesMission();
            default:
                Debug.Log("unknown mission type");
                return new ReachPopulationCountMission(999);
        }
    }

    /*private static int GetNewPopulationGoalNumber()
    {
        int newPopulationGoalNumber = GameStats.GetPopulation() / 10 * 15;
        Debug.Log("new population goal set: " + newPopulationGoalNumber);
        return newPopulationGoalNumber;
    }*/
}
