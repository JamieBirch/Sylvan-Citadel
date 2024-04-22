class ReachPopulationNumberMission : Mission
{
    public int goalNumber;
    
    public override bool CheckFinished()
    {
        return GameStats.GetPopulation() >= goalNumber;
    }

    public override void GiveWording()
    {
        wording = "Reach population " + goalNumber;
    }
}