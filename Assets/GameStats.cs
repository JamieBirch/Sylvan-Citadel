using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int Wood;
    public static int Food;
    public static int Population;
    
    public static List<OwnedHex> OwnedTiles = new List<OwnedHex>();

    public static void AddTile(OwnedHex newTile)
    {
        OwnedTiles.Add(newTile);
    }
    
}
