using System;
using UnityEngine;

public class Human : MonoBehaviour
{
    public bool isHungry = false;
    public bool hasHome = false;
    public string houseTag = "house";
    public string fruitTag = "fruit";
    private GameObject _currentTarget;

    public int speed;

    private GameObject _home;
    
    private void Start()
    {
        GameManager.NewDay += StartDay;
    }

    private void Update()
    {
        if (_currentTarget != null)
        {
            RunToTarget();
        }
        if (isHungry)
        {
            FindFood();
        }
        if (!hasHome)
        {
            FindHome();
        }
    }

    void StartDay()
    {
        if (isHungry)
        {
            Debug.Log("I'm dying of hunger! :(");
            //die
            Destroy(gameObject);
            GameStats.Population--;
        }
        Debug.Log("I'm starting my day!");
        isHungry = true;
        
        //TODO find water

    }

    private void FindHome()
    {
        //TODO find shelter
        GameObject[] houses = GameObject.FindGameObjectsWithTag(houseTag);
        
        //find nearest food
        float shortestDistance = Mathf.Infinity;
        GameObject nearestHouse = null;
        foreach (GameObject house in houses)
        {
            float distanceToHouse = Vector3.Distance(transform.position, house.transform.position);
            if (distanceToHouse < shortestDistance)
            {
                shortestDistance = distanceToHouse;
                nearestHouse = house;
            }
        }

        if (nearestHouse != null)
        {
            _currentTarget = nearestHouse;
        } else
        {
            _currentTarget = null;
        }
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
            if (_currentTarget.CompareTag(houseTag))
            {
                _currentTarget.GetComponent<House>().PlaceHuman(this);
                hasHome = true;
            }

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

    public void OnDestroy()
    {
        GameManager.NewDay -= StartDay;
    }
}
