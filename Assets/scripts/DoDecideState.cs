using UnityEngine;

public class DoDecideState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (human.isRelocating)
        {
            return human.relocate;
        }
        if (human.Satisfied() && !human.hasWork)
        {
            if (Calendar.night && human.hasHome)
            {
                return human.goHome;
            }
            else
            {
                return human.doWander;
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

    public void UseCurrentTarget(Human human)
    {
        throw new System.NotImplementedException();
    }
}
