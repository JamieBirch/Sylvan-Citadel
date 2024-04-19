using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats instance;
    
    public static int Wood = 0;
    public static int Food = 0;
    private static int Population = 0;

    public GameObject populationUi;
    // public Animator populationUiAnimator;
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
        //TODO play animation;
        populationUi.GetComponent<Animator>().Play("increase");
    }
    
    public void RemoveHuman()
    {
        Population--;

        //TODO play animation;
        populationUi.GetComponent<Animator>().Play("decrease");
    }
    
    public static void AddTile(OwnedHex newTile)
    {
        OwnedTiles.Add(newTile);
    }
    
}
