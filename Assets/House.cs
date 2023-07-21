using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int capacity = 5;
    public int bedsAvailable;
    private List<Human> _tenants;

    private void Start()
    {
        _tenants = new List<Human>();
        bedsAvailable = capacity;
    }

    public void PlaceHuman(Human human)
    {
        _tenants.Add(human);
        bedsAvailable--;
    }
}
