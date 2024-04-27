﻿using System.Collections;
using UnityEngine;
using System.Linq;

public class FindFoodState : IHumanState
{
    public string foodSourceTag = "foodSource";
    
    public IHumanState DoState(Human human)
    {
        if (!human.wantsFood)
        {
            if (!human.hasHome)
            {
                return human.findShelter;
            }
            Debug.Log(human.Name + " is satisfied and not sure what to do");
        } else if (human.currentTarget == null)
        {
            FindFood(human);
        }
        else
        {
            human.RunToTarget();
        }

        return human.doWander;
    }
    
    private void FindFood(Human human)
    {
        IEnumerable foodSources = GameObject.FindGameObjectsWithTag(foodSourceTag).Select(fruit => fruit.transform);
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestFoodSource = null;
        
        foreach (Transform foodSource in foodSources)
        {
            if (foodSource.TryGetComponent<Fruit>(out Fruit fruitComponent))
            {
                if (!fruitComponent.isClaimed)
                {
                    float distanceToFood = Vector3.Distance(human.transform.position, foodSource.position);
                    if (distanceToFood < shortestDistance)
                    {
                        shortestDistance = distanceToFood;
                        nearestFoodSource = fruitComponent.gameObject;
                    }
                }
            }
            else
            {
                float distanceToFood = Vector3.Distance(human.transform.position, foodSource.position);
                if (distanceToFood < shortestDistance)
                {
                    shortestDistance = distanceToFood;
                    nearestFoodSource = foodSource.gameObject;
                }
            }
        }

        if (nearestFoodSource != null)
        {
            human.currentTarget = nearestFoodSource;
            Fruit fruitComponent;
            if (nearestFoodSource.TryGetComponent<Fruit>(out fruitComponent))
            {
                fruitComponent.isClaimed = true;
            }
            // Debug.Log(name + " claimed food");
        } else
        {
            human.currentTarget = null;
        }
    }

    public void UseCurrentTarget(Human human)
    {
        Consume(human);
    }
    
    private void Consume(Human human)
    {
        if (GameStats.GetFood() > 0)
        {
            if (human.currentTarget.TryGetComponent<Fruit>(out _))
            {
                human.DestroyCurrentTarget();
                //TODO visual effect
            }
            human.currentTarget = null;
            GameStats.instance.RemoveFood();

            human.wantsFood = false;
        }
        
    }
}