using UnityEngine;

public class Human : MonoBehaviour
{
    public string Name;
    
    public bool isThirsty = false;
    public bool isHungry = false;
    public bool hasHome = false;
    public string houseTag = "house";
    public string fruitTag = "fruit";
    public string waterTag = "water";
    public GameObject _currentTarget;

    public int speed;

    private GameObject _home;
    
    private void Start()
    {
        GameManager.NewDay += StartDay;
    }

    private void Update()
    {
        if (isThirsty)
        {
            FindWater();
            if (_currentTarget != null)
            {
                RunToTarget();
            }
        }
        if (isHungry)
        {
            FindFood();
            if (_currentTarget != null)
            {
                RunToTarget();
            }
        }
        if (!hasHome)
        {
            FindHome();
            if (_currentTarget != null)
            {
                RunToTarget();
            }
        }
    }

    private void FindWater()
    {
        GameObject[] waterSources = GameObject.FindGameObjectsWithTag(waterTag);
        
        //find nearest food
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

    void StartDay()
    {
        if (isHungry || isThirsty)
        {
            Debug.Log("I'm dying! :(");
            //die
            Destroy(gameObject);
            GameStats.Population--;
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
        
        if (dir.magnitude <= distanceThisFrame)
        {
            if (_currentTarget.CompareTag(fruitTag))
            {
                Consume();
            } else if (_currentTarget.CompareTag(houseTag))
            {
                _currentTarget.GetComponent<House>().PlaceHuman(this);
                hasHome = true;
            } else if (_currentTarget.CompareTag(waterTag))
            {
                Drink();
            }

            _currentTarget = null;
        }
    }

    private void Drink()
    {
        //TODO effect
        isThirsty = false;
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
