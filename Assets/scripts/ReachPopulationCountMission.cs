using UnityEngine;

class ReachPopulationCountMission : Mission
{
    public int goalNumber;
    
    public ReachPopulationCountMission(int goalNumber)
    {
        this.goalNumber = goalNumber;
    }
    
    public ReachPopulationCountMission() : this(GetNewPopulationGoalNumber())
    {
    }
    
    public override bool CheckFinished()
    {
        return GameStats.GetPopulation() >= goalNumber;
    }

    public override string GiveWording()
    {
        return "Reach population " + goalNumber;
    }
    
    private static int GetNewPopulationGoalNumber()
    {
        int newPopulationGoalNumber = (GameStats.GetPopulation() / 10 + 1) * 15;
        if (newPopulationGoalNumber > 100)
        {
            newPopulationGoalNumber = 100;
        }
        Debug.Log("new population goal set: " + newPopulationGoalNumber);
        
        return newPopulationGoalNumber;
    }
}