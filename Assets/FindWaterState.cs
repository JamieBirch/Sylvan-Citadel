using System.Collections.Generic;
using UnityEngine;

public class FindWaterState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (!human.isThirsty)
        {
            if (human.isHungry)
            {
                return human.findFood;
            } else if (!human.hasHome)
            {
                return human.findShelter;
            }
            Debug.Log(human.Name + " is satisfied and not sure what to do");
        } else if (human.currentTarget == null)
        {
            FindWater(human);
        }
        else
        {
            human.RunToTarget();
        }

        return this;
    }

    public void UseCurrentTarget(Human human)
    {
        Drink(human);
    }

    private void FindWater(Human human)
    {
        GameObject homeHexWaterway = human.homeHex.waterway;

        //TODO replace
        List<Lake> waterSources = homeHexWaterway.GetComponent<Waterway>().lakes;
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestWater = null;
        foreach (Lake waterSource in waterSources)
        {
            float distanceToWater = Vector3.Distance(human.transform.position, waterSource.transform.position);
            if (distanceToWater < shortestDistance)
            {
                shortestDistance = distanceToWater;
                nearestWater = waterSource.gameObject;
            }
        }

        if (nearestWater != null)
        {
            human.currentTarget = nearestWater;
        } else
        {
            human.currentTarget = null;
        }
    }
    
    private void Drink(Human human)
    {
        //TODO effect
        human.isThirsty = false;
        human.currentTarget = null;
    }
}
