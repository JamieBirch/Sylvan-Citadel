public class RelocateState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (human.currentTarget == null)
        {
            //Set satisfied,
            human.wantsWater = false;
            human.wantsFood = false;
            //Set target as newHex
            human.currentTarget = human.homeTile.gameObject;
        }
        else
        {
            human.wantsWater = false;
            human.wantsFood = false;
            human.RunToTarget();
            return human.decide;
        }

        return human.relocate;
    }

    public void UseCurrentTarget(Human human)
    {
        human.currentTarget = null;
        human.isRelocating = false;
    }

    public string GetStateString()
    {
        return "Moving to ";
    }
}
