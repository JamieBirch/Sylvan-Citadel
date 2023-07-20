using System;
using UnityEngine;

public class Human : MonoBehaviour
{
    private bool isHungry = false;
    public string fruitTag = "fruit";
    private GameObject _currentTarget;

    public int speed = 3;

    
    private void Start()
    {
        GameManager.newDay += StartDay;
    }

    private void Update()
    {
        if (_currentTarget != null)
        {
            RunToTarget();
        }
    }

    private void RunToTarget()
    {
        Vector3 dir = new Vector3(
            _currentTarget.transform.position.x - transform.position.x, 
            0,
            _currentTarget.transform.position.z - transform.position.z);
        float distanceThisFrame = speed * Time.deltaTime;
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_currentTarget.transform);
        
        if (dir.magnitude <= distanceThisFrame)
        {
            if (_currentTarget.CompareTag(fruitTag))
            {
                Consume();
            }
        }
    }

    void StartDay()
    {
        Debug.Log("I'm starting my day!");
        isHungry = true;
        
        //TODO find food
        if (isHungry)
        {
            FindFood();
        }
        
        //TODO find water
        //TODO find shelter
    }

    private void FindFood()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag(fruitTag);
        
        //find nearest food
        float shortestDistance = Mathf.Infinity;
        GameObject nearestFood = null;
        foreach (GameObject fruit in fruits)
        {
            float distanceToFood = Vector3.Distance(transform.position, fruit.transform.position);
            if (distanceToFood < shortestDistance)
            {
                shortestDistance = distanceToFood;
                nearestFood = fruit;
            }
        }

        if (nearestFood != null)
        {
            _currentTarget = nearestFood;
        } else
        {
            _currentTarget = null;
        }
    }
    
    private void Consume()
    {
        Destroy(_currentTarget);
        //TODO effect

        isHungry = false;

        GameStats.FruitsAvailable--;
    }
}
