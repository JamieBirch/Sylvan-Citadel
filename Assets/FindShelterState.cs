using UnityEngine;

public class FindShelterState : IHumanState
{
    public string houseTag = "house";

    public IHumanState DoState(Human human)
    {
        /*if (human.currentTarget == null)
        {
            FindHome(human);
            
        }
        else
        {
            human.RunToTarget();
            return human.doWander;
        }

        return this;*/

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
        // return this;
    }
    
    private GameObject FindHome(Human human)
    {
        
        //FIXME fix overcrowded houses
        // GameObject[] houses = GameObject.FindGameObjectsWithTag(houseTag);
        
        // GameObject homeHex = human.homeHex.gameObject;
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestHouse = null;
        GameObject[] houseGamebjects = GameObject.FindGameObjectsWithTag(houseTag);
        // foreach (Transform child in homeHex.transform)
        foreach (GameObject houseGO in houseGamebjects)
        {
            // if (child.CompareTag(houseTag))
            // {
                House thisHouse = houseGO.GetComponent<House>();
                if (thisHouse.bedsAvailable > 0)
                {
                    float distanceToHouse = Vector3.Distance(human.transform.position, houseGO.transform.position);
                    if (distanceToHouse < shortestDistance)
                    {
                        shortestDistance = distanceToHouse;
                        nearestHouse = houseGO.gameObject;
                    }
                }
            // }
        }

        if (nearestHouse != null)
        {
            // human.currentTarget = nearestHouse;
            return nearestHouse;
        } else
        {
            // human.currentTarget = null;
            return null;
        }
    }

    public void UseCurrentTarget(Human human)
    {
        bool movedIn = human.currentTarget.GetComponent<House>().MoveIn(human);
        if (movedIn)
        {
            human._home = human.currentTarget;
            human.hasHome = true;
            human._home.GetComponent<House>().hex.SettleInHex(human);
        }
        human.currentTarget = null;
    }
}