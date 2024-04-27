using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DoWanderState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (human.isRelocating)
        {
            return human.relocate;
        }
        if (human.Satisfied() && !human.hasWork)
        {
            if (Calendar.night)
            {
                return human.goHome;
            }
            else
            {
                Wander(human);
            }
        }
        else if(!human.Satisfied())
        {
            if (human.wantsWater)
            {
                return human.findWater;
            } else if (human.wantsFood)
            {
                return human.findFood;
            } else if (!human.hasHome)
            {
                return human.findShelter;
            }
        }
        else
        {
            Debug.Log(human.name + " is ready to work");
            return human.doWork;
        }

        return this;
    }

    private void Wander(Human human)
    {
        if (human.currentTarget == null)
        {
            Array values = Enum.GetValues(typeof(SparetimeActivity));
            SparetimeActivity randomActivity = (SparetimeActivity)values.GetValue(new Random().Next(values.Length));

            switch (randomActivity)
            {
                case SparetimeActivity.admireBuilding:
                {
                    List<Building> buildings = human.homeHex.buildings;
                    human.currentTarget = buildings[new Random().Next(buildings.Count - 1)].gameObject;
                    break;
                }
                case SparetimeActivity.followPerson:
                {
                    List<Human> humans = human.homeHex.village.GetComponent<Village>().humans;
                    human.currentTarget = humans[new Random().Next(humans.Count - 1)].gameObject;
                    break;
                }
                case SparetimeActivity.hugTree:
                {
                    if (human.homeHex.GetWoodland() != null)
                    {
                        List<Tree> trees = human.homeHex.GetWoodland().trees;
                        human.currentTarget = trees[new Random().Next(trees.Count - 1)].gameObject;
                    }
                    break;
                }
                default:
                    human.currentTarget = null;
                    break;
            }
        }
        else
        {
            human.RunToTarget();
        }
    }

    private enum SparetimeActivity
    {
        admireBuilding,
        followPerson,
        hugTree
    }
    
    public void UseCurrentTarget(Human human)
    {
        // Debug.Log("can't utilize target in wander state");
        human.currentTarget = null;
    }
}