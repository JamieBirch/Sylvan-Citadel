using System.Collections.Generic;

public class Monarch
{
    public string Name;
    public Boon boon;
    // public Gaese gaese;
    public List<Mission> missions;

    public Monarch(string name, Boon boon, List<Mission> missions)
    {
        this.Name = name;
        this.boon = boon;
        this.missions = missions;
    }
    
}