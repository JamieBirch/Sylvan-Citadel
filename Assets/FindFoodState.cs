using UnityEngine;

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
        GameObject homeHex = human.homeHex.gameObject;

        //TODO replace
        GameObject _woodland = homeHex.GetComponent<OwnedHex>().woodland;

        //find nearest food
        float shortestDistance = Mathf.Infinity;
        GameObject nearestFood = null;
        foreach (Transform child in _woodland.transform)
        {
            if (child.CompareTag(fruitTag))
            {
                if (!child.GetComponent<Fruit>().isClaimed)
                {
                    float distanceToFood = Vector3.Distance(human.transform.position, child.transform.position);
                    if (distanceToFood < shortestDistance)
                    {
                        shortestDistance = distanceToFood;
                        nearestFood = child.gameObject;
                    }
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
        GameStats.FruitsAvailable--;

        human.isHungry = false;
    }
}