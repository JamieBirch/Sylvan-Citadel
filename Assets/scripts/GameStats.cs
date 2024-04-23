using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats instance;
    
    private static int Wood = 0;
    private static int Food = 0;
    private static int Population = 0;

    public GameObject populationUi;
    public GameObject woodUi;
    public GameObject foodUi;
    
    public static List<OwnedHex> OwnedTiles = new List<OwnedHex>();

    private void Awake()
    {
        instance = this;
    }
    
    public static int GetPopulation()
    {
        return Population;
    }
    
    public void AddHuman()
    {
        Population++;
        populationUi.GetComponent<Animator>().Play("increase");
    }
    
    public void RemoveHuman()
    {
        Population--;
        populationUi.GetComponent<Animator>().Play("decrease");
    }
    
    public static int GetWood()
    {
        return Wood;
    }
    
    public void AddWood(int count)
    {
        Wood+=count;
        woodUi.GetComponent<Animator>().Play("increase");
    }
    
    public void RemoveWood(int count)
    {
        Wood-=count;
        woodUi.GetComponent<Animator>().Play("decrease");
    }
    
    public static int GetFood()
    {
        return Food;
    }
    
    public void AddFood()
    {
        Food++;
        foodUi.GetComponent<Animator>().Play("increase");
    }
    
    public void RemoveFood()
    {
        Food--;
        foodUi.GetComponent<Animator>().Play("decrease");
    }
    
    public static void AddTile(OwnedHex newTile)
    {
        OwnedTiles.Add(newTile);
    }

    public void AddFood(int startStorageFood)
    {
        Food+= startStorageFood;
    }
}
