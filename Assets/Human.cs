using UnityEngine;
using UnityEngine.Serialization;

public class Human : MonoBehaviour
{
    private PopulationManager _populationManager;
    
    public string Name;
    
    public bool isThirsty = false;
    public bool isHungry = false;
    public bool hasHome = false;
    public string houseTag = "house";
    public string fruitTag = "fruit";
    public string waterTag = "water";
    public GameObject _currentTarget;

    public int speed;
    
    public int fertility;
    public Vector3 offset = new Vector3(0.05f, 0.1f, 0.05f);

    private GameObject _home;
    
    private void Start()
    {
        GameManager.NewDay += StartDay;
        _populationManager = PopulationManager.instance;
    }

    private void Update()
    {
        if (_currentTarget == null)
        {
            if (isThirsty)
            {
                FindWater();
            } else if (isHungry)
            {
                FindFood();
            } else if (!hasHome)
            {
                FindHome();
            }
            else
            {
                GoHome();
            }
        }
        else
        {
            RunToTarget();
        }
    }

    private void GoHome()
    {
        // _currentTarget = _home;
        Vector3 dir = new Vector3(
            _home.transform.position.x - transform.position.x, 
            0,
            _home.transform.position.z - transform.position.z);
        float distanceThisFrame = speed * Time.deltaTime;
        
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_home.transform);
        
        // RunToTarget();
    }

    private void FindWater()
    {
        GameObject[] waterSources = GameObject.FindGameObjectsWithTag(waterTag);
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestWater = null;
        foreach (GameObject waterSource in waterSources)
        {
            float distanceToWater = Vector3.Distance(transform.position, waterSource.transform.position);
            if (distanceToWater < shortestDistance)
            {
                shortestDistance = distanceToWater;
                nearestWater = waterSource;
            }
        }

        if (nearestWater != null)
        {
            _currentTarget = nearestWater;
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
            if (!fruit.GetComponent<Fruit>().isClaimed)
            {
                float distanceToFood = Vector3.Distance(transform.position, fruit.transform.position);
                if (distanceToFood < shortestDistance)
                {
                    shortestDistance = distanceToFood;
                    nearestFood = fruit;
                }
            }
        }

        if (nearestFood != null)
        {
            _currentTarget = nearestFood;
            nearestFood.GetComponent<Fruit>().isClaimed = true;
            Debug.Log(name + " claimed food");
        } else
        {
            _currentTarget = null;
        }
    }

    private void FindHome()
    {
        GameObject[] houses = GameObject.FindGameObjectsWithTag(houseTag);
        
        float shortestDistance = Mathf.Infinity;
        GameObject nearestHouse = null;
        foreach (GameObject house in houses)
        {
            House thisHouse = house.GetComponent<House>();
            if (thisHouse.bedsAvailable > 0)
            {
                float distanceToHouse = Vector3.Distance(transform.position, house.transform.position);
                if (distanceToHouse < shortestDistance)
                {
                    shortestDistance = distanceToHouse;
                    nearestHouse = house;
                }
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

    void StartDay()
    {
        if (isHungry || isThirsty)
        {
            Debug.Log("I'm dying! :(");
            //die
            Destroy(gameObject);
            GameStats.Population--;
        }

        if (GameStats.BedsAvailable > 0)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance <= fertility)
            {
                GiveBirth();
            }
        }
        
        Debug.Log("I'm starting my day!");
        isHungry = true;
        isThirsty = true;
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
        
        if (dir.magnitude  <= distanceThisFrame)
        {
            if (_currentTarget.CompareTag(fruitTag))
            {
                Consume();
            } else if (_currentTarget.CompareTag(houseTag))
            {
                _currentTarget.GetComponent<House>().PlaceHuman(this);
                _home = _currentTarget;
                hasHome = true;
                _currentTarget = null;
            } else if (_currentTarget.CompareTag(waterTag))
            {
                Drink();
            }
        }
    }

    private void Drink()
    {
        //TODO effect
        isThirsty = false;
        _currentTarget = null;
    }

    private void Consume()
    {
        Destroy(_currentTarget);
        _currentTarget = null;
        //TODO effect
        GameStats.FruitsAvailable--;

        isHungry = false;
    }

    private void GiveBirth()
    {
        Debug.Log("I'm having a child!");
        _populationManager.SpawnHuman(gameObject.transform.position + offset);
    }

    public void OnDestroy()
    {
        GameManager.NewDay -= StartDay;
    }
}
