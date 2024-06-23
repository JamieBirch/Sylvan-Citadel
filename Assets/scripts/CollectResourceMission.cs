using System;
using UnityEngine;

class CollectResourceMission : Mission
{
    public int goalNumber;
    // public string resourceName = "stored food";
    public CollectableResource resource;
    
    public CollectResourceMission(int goalNumber, CollectableResource resource)
    {
        this.goalNumber = goalNumber;
        this.resource = resource;
    }
    
    public CollectResourceMission() :this(GetNewResourceGoalNumber(), Utils.RandomEnumValue<CollectableResource>())
    {
    }
    
    public override bool CheckFinished()
    {
        return (resource.GetResourceStatsCount() >= goalNumber);
    }

    public override string GiveWording()
    {
        return "collect " + goalNumber + " of " + resource.GetResourceName();
    }
    
    private static int GetNewResourceGoalNumber()
    {
        int newResourceGoalNumber = GameStats.GetWood()/ 100 + 200;
        Debug.Log("new resource goal set: " + newResourceGoalNumber);
        return newResourceGoalNumber;
    }
}

public enum CollectableResource
{
    wood,
    food
}

static class StuffResources
{
    public static String GetResourceName(this CollectableResource resource)
    {
        switch (resource)
        {
            case CollectableResource.wood:
                return "wood";
            case CollectableResource.food:
                return "stored food";
            default:
                return "unknown resource type";
        }
    }
    
    public static int GetResourceStatsCount(this CollectableResource resource)
    {
        switch (resource)
        {
            case CollectableResource.wood:
                return GameStats.GetWood();
            case CollectableResource.food:
                return GameStats.GetFood();
            default:
                Debug.Log("unknown resource type");
                return 99999999;
        }
    }
}