using System;
using System.Collections.Generic;
using System.Linq;
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
        return Utils.RandomEnumValue<MissionType>().GetMission();
    }
    
    public Mission GenerateMission(MissionType missionType)
    {
        return missionType.GetMission();
    }

    public MissionType[] PickRandomMissionTypes(int missionCount)
    {
        IEnumerable<MissionType> pickedMissionTypes = Enum.GetValues(typeof(MissionType))
            .OfType<MissionType>()
            .OrderBy(e => Guid.NewGuid())
            .Take(missionCount);
        return pickedMissionTypes.ToArray();
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
