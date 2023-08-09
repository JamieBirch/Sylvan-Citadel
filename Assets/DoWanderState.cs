using UnityEngine;

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
            return human.goHome;
        }
        else if(!human.Satisfied())
        {
            if (human.isThirsty)
            {
                return human.findWater;
            } else if (human.isHungry)
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

    public void UseCurrentTarget(Human human)
    {
        Debug.Log("can't utilize target in wander state");
    }
}