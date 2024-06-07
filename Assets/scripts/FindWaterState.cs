using System.Collections;
using UnityEngine;

public class FindWaterState : IHumanState
{
    public string waterTag = "water";
    
    public IHumanState DoState(Human human)
    {
        if (!human.wantsWater)
        {
            /*if (human.wantsFood)
            {
                return human.findFood;
            } else if (!human.hasHome)
            {
                return human.findShelter;
            }
            Debug.Log(human.Name + " is satisfied and not sure what to do");*/
            return human.decide;
        } else {
            GameObject nearestWater = FindWater(human);
            if (nearestWater != null)
            {
                human.currentTarget = nearestWater;
                human.RunToTarget();
                return this;
                // return human.doWander;
            }
        }
        return this;
    }

    public void UseCurrentTarget(Human human)
    {
        Drink(human);
    }

    private GameObject FindWater(Human human)
    {
        IEnumerable lakes;
        {
            lakes = GameObject.FindGameObjectsWithTag(waterTag);
        }
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestWater = null;
        foreach (var waterSource in lakes)
        {
            float distanceToWater = Vector3.Distance(human.transform.position, ((GameObject)waterSource).transform.position);
            if (distanceToWater < shortestDistance)
            {
                shortestDistance = distanceToWater;
                nearestWater = (GameObject)waterSource;
            }
        }

        if (nearestWater != null)
        {
            return nearestWater;
        } else
        {
            return null;
        }
    }
    
    private void Drink(Human human)
    {
        //TODO visual effect
        human.wantsWater = false;
        human.currentTarget = null;
    }
}
