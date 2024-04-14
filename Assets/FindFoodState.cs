using System.Collections;
using UnityEngine;
using System.Linq;

public class FindFoodState : IHumanState
{
    public string fruitTag = "fruit";
    public IHumanState DoState(Human human)
    {
        if (!human.isHungry)
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
        IEnumerable fruits;
        fruits = GameObject.FindGameObjectsWithTag(fruitTag).Select(fruit => fruit.transform);
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestFood = null;
        
        foreach (Transform fruit in fruits)
        {
            if (!fruit.GetComponent<Fruit>().isClaimed)
            {
                float distanceToFood = Vector3.Distance(human.transform.position, fruit.transform.position);
                if (distanceToFood < shortestDistance)
                {
                    shortestDistance = distanceToFood;
                    nearestFood = fruit.gameObject;
                }
            }
        }

        if (nearestFood != null)
        {
            human.currentTarget = nearestFood;
            nearestFood.GetComponent<Fruit>().isClaimed = true;
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
        human.DestroyCurrentTarget();
        human.currentTarget = null;
        //TODO effect
        GameStats.Food--;
        // human.homeHex.FruitsAvailable--;

        human.isHungry = false;
    }
}