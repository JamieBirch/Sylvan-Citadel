class ReachPopulationNumberMission : Mission
{
    public int goalNumber = 30;
    
    public override bool CheckFinished()
    {
        return GameStats.GetPopulation() >= goalNumber;
    }

    public override string GiveWording()
    {
        return "Reach population " + goalNumber;
    }
}