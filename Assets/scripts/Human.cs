using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    private PopulationManager _populationManager;

    // public Sprite footprint;
    public Canvas footprint;
    public string Name;
    
    public Color unsatisfiedColor;
    public Color satisfiedColor;
    public Renderer rend;
    
    public bool wantsWater = false;
    public bool wantsFood = false;
    public bool hasHome = false;
    public bool isRelocating = false;
    public GameObject currentTarget;
    [FormerlySerializedAs("homeHex")] public OwnedTile homeTile;

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
    public RelocateState relocate = new RelocateState();

    public GameObject VillagerInfoPanel;
    public Text NameText;
    public Text ThirstyText;
    public Text HungryText;
    public Text HomelessText;
    public Text ThoughtText;
    public Text StateText;
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
        _populationManager = PopulationManager.instance;
        state = doWander;
        NameText.text = Name;
    }

    private void Update()
    {
        state = state.DoState(this);
        stateName = state.ToString();
        StateText.text = stateName;
        
        if (Satisfied())
        {
            speed = 1;
            rend.material.color = satisfiedColor;
        }
        else
        {
            speed = 2;
            rend.material.color = unsatisfiedColor;
        }
    }
    
    void OnMouseDown()
    {
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/
        // else
        // {
        VillagerInfoPanel.SetActive(true);
        // }
    }

    void StartDay()
    {
        if (wantsFood || wantsWater)
        {
            Die();
        }

        if (GameStats.GetPopulation() < _populationManager.maxPopulation)
        {
            if ((HasAvailableBeds() || homeTile.NeighborsHaveAvailableBeds()) && Satisfied())
            {
                double chance = Utils.GenerateRandomChance();
                if (chance <= fertility)
                {
                    GiveBirth();
                }
            }
        }
        else
        {
            Debug.Log("Reached max population");
        }
        
        // Debug.Log("I'm starting my day!");
        wantsFood = true;
        wantsWater = true;
    }

    private bool HasAvailableBeds()
    {
        return homeTile.GetBedsAvailable() > 0;
    }

    public void Hire()
    {
        //TODO
        hasWork = true;
    }

    public void RunToTarget()
    {
        // DoFootprint();
        
        Transform humanTransform = transform;
        Transform currentTargetTransform = currentTarget.transform;

        Vector3 dir = new Vector3(
            currentTargetTransform.position.x - humanTransform.position.x, 
            0,
            currentTargetTransform.position.z - humanTransform.position.z);
        float distanceThisFrame = speed * Time.deltaTime;

        humanTransform.Translate(dir.normalized * distanceThisFrame, Space.World);
        // humanTransform.LookAt(currentTargetTransform);

        float distance = Vector3.Distance(humanTransform.position, currentTargetTransform.position);
        
        if (distance <= 0.4)
        {
            state.UseCurrentTarget(this);
        }
    }

    private void DoFootprint()
    {
        // RaycastHit hit;

        // if (Physics.Raycast(transform.position, transform.forward, out hit))
        // {
            Instantiate(footprint, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        // }
    }

    public bool Satisfied()
    {
        return !wantsWater && !wantsFood && hasHome;
    }

    public void DestroyCurrentTarget()
    {
        Destroy(currentTarget);
    }

    private void GiveBirth()
    {
        Debug.Log("I'm having a child!");
        _populationManager.SpawnHuman(gameObject.GetComponentInParent<Village>().gameObject);
    }

    public void Die()
    {
        Debug.Log("I'm dying! :(");
        //die
        if (_home != null)
        {
            MoveOut();
        }
        homeTile.village.GetComponent<Village>().humans.Remove(this);
        Destroy(gameObject);
        homeTile.HexPopulation--;
        GameStats.instance.RemoveHuman();
    }

    public void MoveOut()
    {
        _home.GetComponent<House>().MoveOut(this);
        hasHome = false;
        _home = null;
    }

    public void MoveIn(House house)
    {
        _home = house.gameObject;
        hasHome = true;
       _home.GetComponent<House>().tile.SettleInHex(this);
    }

    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
