
public class GoHomeState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        if (human.currentTarget == null)
        {
            SetHomeAsTarget(human);
        }
        else
        {
            human.RunToTarget();
            return this;
        }
        return human.decide;
    }

    private void SetHomeAsTarget(Human human)
    {
        human.currentTarget = human._home;
    }

    public void UseCurrentTarget(Human human)
    {
        // Debug.Log("I'm home");
    }
}
