public class RelocateState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (human.currentTarget == null)
        {
            //Set satisfied,
            human.isThirsty = false;
            human.isHungry = false;
            //Set target as newHex
            human.currentTarget = human.homeHex.gameObject;
        }
        else
        {
            human.isThirsty = false;
            human.isHungry = false;
            human.RunToTarget();
            return human.doWander;
        }

        return human.relocate;
    }

    public void UseCurrentTarget(Human human)
    {
        human.currentTarget = null;
        human.isRelocating = false;
    }
}
