using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWaterState : IHumanState
{
    public string waterTag = "water";
    
    public IHumanState DoState(Human human)
    {
        if (!human.wantsWater)
        {
            if (human.wantsFood)
            {
                return human.findFood;
            } else if (!human.hasHome)
            {
                return human.findShelter;
            }
            Debug.Log(human.Name + " is satisfied and not sure what to do");
        } else {
            GameObject nearestWater = FindWater(human);
            if (nearestWater != null)
            {
                human.currentTarget = nearestWater;
                human.RunToTarget();
                return human.doWander;
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
        /*GameObject homeHexWaterway = human.homeHex.waterway;

        if (homeHexWaterway != null)
        {
            Waterway waterway = homeHexWaterway.GetComponent<Waterway>();
            lakes = waterway.lakes;
        }
        else*/
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
            // human.currentTarget = nearestWater;
            return nearestWater;
        } else
        {
            // human.currentTarget = null;
            return null;
        }
    }
    
    private void Drink(Human human)
    {
        //TODO effect
        human.wantsWater = false;
        human.currentTarget = null;
    }
}
