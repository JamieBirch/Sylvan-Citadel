class ReachPopulationNumberMission : Mission
{
    public int goalNumber;
    
    public override bool CheckFinished()
    {
        return GameStats.Population >= goalNumber;
    }

    public override void GiveWording()
    {
        wording = "Reach population " + goalNumber;
    }
}