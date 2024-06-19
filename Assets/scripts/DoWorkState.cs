public class DoWorkState : IHumanState
{
    public IHumanState DoState(Human human)
    {
        //TODO
        return human.decide;
    }

    public void UseCurrentTarget(Human human)
    {
        //TODO
    }

    public string GetStateString()
    {
        return "Working";
    }
}
