using System;
using UnityEngine;

public class Human : MonoBehaviour
{
    private PopulationManager _populationManager;
    
    public string Name;
    
    public bool isThirsty = false;
    public bool isHungry = false;
    public bool hasHome = false;
    public GameObject currentTarget;
    public OwnedHex homeHex;

    public bool hasWork = false;
    public int speed;
    public int fertility;

    public GameObject _home;
    private IHumanState state;
    public String stateName;
    public FindWaterState findWater = new FindWaterState();
    public FindFoodState findFood = new FindFoodState();
    public FindShelterState findShelter = new FindShelterState();
    public DoWanderState doWander = new DoWanderState();
    public GoHomeState goHome = new GoHomeState();
    public DoWorkState doWork = new DoWorkState();
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
        _populationManager = PopulationManager.instance;
        state = doWander;
    }

    private void Update()
    {
        state = state.DoState(this);
        stateName = state.ToString();
    }

    void StartDay()
    {
        if (isHungry || isThirsty)
        {
            Die();
        }

        if (GameStats.BedsAvailable > 0 && Satisfied())
        {
            double chance = Utils.GenerateRandomChance();
            if (chance <= fertility)
            {
                GiveBirth();
            }
        }
        
        // Debug.Log("I'm starting my day!");
        isHungry = true;
        isThirsty = true;
    }

    private void Die()
    {
        Debug.Log("I'm dying! :(");
        //die
        if (_home != null)
        {
            _home.GetComponent<House>().MoveOut(this);
        }
        Destroy(gameObject);
        GameStats.Population--;
    }

    public void RunToTarget()
    {
        Transform humanTransform = transform;
        Transform currentTargetTransform = currentTarget.transform;

        Vector3 dir = new Vector3(
            currentTargetTransform.position.x - humanTransform.position.x, 
            0,
            currentTargetTransform.position.z - humanTransform.position.z);
        float distanceThisFrame = speed * Time.deltaTime;

        humanTransform.Translate(dir.normalized * distanceThisFrame, Space.World);
        humanTransform.LookAt(currentTargetTransform);

        float distance = Vector3.Distance(humanTransform.position, currentTargetTransform.position);
        
        if (distance <= 0.4)
        {
            state.UseCurrentTarget(this);
        }
    }

    private void GiveBirth()
    {
        Debug.Log("I'm having a child!");
        _populationManager.SpawnHuman(gameObject.GetComponentInParent<Village>().gameObject);
    }

    public bool Satisfied()
    {
        return !isThirsty && !isHungry && hasHome;
    }
    
    public void DestroyCurrentTarget()
    {
        Destroy(currentTarget);
    }

    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
