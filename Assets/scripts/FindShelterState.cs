using UnityEngine;

public class FindShelterState : IHumanState
{
    public string houseTag = "house";

    public IHumanState DoState(Human human)
    {
        GameObject nearestHouse = FindHome(human);
        if (nearestHouse != null)
        {
            human.currentTarget = nearestHouse;
            human.RunToTarget();
            return human.doWander;
        }
        else
        {
            human.currentTarget = null;
            return human.doWander;
        }
    }
    
    private GameObject FindHome(Human human)
    {
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestHouse = null;
        GameObject[] houseGamebjects = GameObject.FindGameObjectsWithTag(houseTag);
        foreach (GameObject houseGO in houseGamebjects)
        {
                House thisHouse = houseGO.GetComponent<House>();
                if (thisHouse.GetBedsAvailable() > 0)
                {
                    float distanceToHouse = Vector3.Distance(human.transform.position, houseGO.transform.position);
                    if (distanceToHouse < shortestDistance)
                    {
                        shortestDistance = distanceToHouse;
                        nearestHouse = houseGO.gameObject;
                    }
                }
        }

        if (nearestHouse != null)
        {
            return nearestHouse;
        } else
        {
            return null;
        }
    }

    public void UseCurrentTarget(Human human)
    {
        House house = human.currentTarget.GetComponent<House>();
        bool movedIn = house.MoveIn(human);
        if (movedIn)
        {
            human.MoveIn(house);
        }
        human.currentTarget = null;
    }
}