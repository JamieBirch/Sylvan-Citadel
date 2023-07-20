using UnityEngine;

using Random = System.Random;


public class Tree : MonoBehaviour
{

    public static int Fertility = 80;

    private void Start()
    {
        GameManager.newDay += StartDay;
    }
    
    void StartDay()
    {
        //TODO spawn fruit with 80% chance
        double chance = new Random().NextDouble() * 100;
        if (chance <= Fertility)
        {
            Debug.Log("I feel fruity today!");
            GameStats.FruitsAvailable++;
        }
    }
}
