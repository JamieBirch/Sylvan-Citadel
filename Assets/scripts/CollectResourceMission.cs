class CollectResourceMission : Mission
{
    public int goalNumber = 100;
    public string resourceName = "stored food";
    
    public override bool CheckFinished()
    {
        return (GameStats.GetFood() >= 100);
    }

    public override string GiveWording()
    {
        return "collect " + goalNumber + " of " + resourceName;
    }
}