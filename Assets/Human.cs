using UnityEngine;

public class Human : MonoBehaviour
{
    private void Start()
    {
        GameManager.newDay += StartDay;
    }


    void StartDay()
    {
        Debug.Log("I'm starting my day!");
        
        //TODO find food
        //TODO find water
        //TODO find shelter
    }
}
