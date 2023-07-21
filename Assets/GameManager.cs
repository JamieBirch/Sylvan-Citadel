using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GameObject sun;
    
    public GameObject tree;
    public GameObject human;
    
    public int StartHumans;
    public int StartTrees;
    
    //initial resources
    public int StartStorageWood = 50;
    public int StartStorageFood = 100;

    public Vector3 firstTileCenter = Vector3.zero;
    public float HexRadius;

    public static int day;
    public float countdown;
    public float dayLength = 60f;

    public Text daysText;
    public Text countdownText;
    public Text fruits;
    public Text humans;

    public static event Action NewDay;

    // Start is called before the first frame update
    void Start()
    {
        GameStats.Population = 0;
        
        // spawn fruit trees
        SpawnTrees();

        // put storage resources to storage
        GameStats.Food = StartStorageFood;
        GameStats.Wood = StartStorageWood;

        // spawn humans
        SpawnHumans();
        

        // start world time
        countdown = dayLength;
        day = 1;
        
        NewDay?.Invoke();
    }

    private void SpawnHumans()
    {
        for (int i = 0; i < StartHumans; i++)
        {
            SpawnHuman();
        }
    }

    private void SpawnHuman()
    {
        var position = PositionOnHex(firstTileCenter) + new Vector3(0, 1.25f, 0);
        Instantiate(human, position, Quaternion.identity);
        GameStats.Population++;
    }

    // Update is called once per frame
    void Update()
    {
        fruits.text = GameStats.FruitsAvailable.ToString();
        humans.text = GameStats.Population.ToString();
        
        var sunTransform = sun.transform;

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        sunTransform.Rotate(Time.deltaTime * (360/dayLength),0, 0);
        
        if (countdown <= 0)
        {
            //start new day
            day++;
            countdown = dayLength;
            sunTransform.rotation = Quaternion.Euler(0, -60, 0);
            NewDay?.Invoke();
        }

        countdownText.text = string.Format("{0:00.00}", countdown);
        daysText.text = day.ToString();
        
    }

    void SpawnTrees()
    {
        for (int i = 0; i < StartTrees; i++)
        {
            SpawnTree();
        }
    }

    private void SpawnTree()
    {
        var position = PositionOnHex(firstTileCenter) + new Vector3(0, 1f, 0);
        Instantiate(tree, position, Quaternion.identity);
    }

    private Vector3 PositionOnHex(Vector3 hexCenter)
    {
        float minX = hexCenter.x - HexRadius;
        float maxX = hexCenter.x + HexRadius;
        
        float minZ = hexCenter.z - HexRadius;
        float maxZ = hexCenter.z + HexRadius;

        return new Vector3(generateRandom(minX, maxX), 0f, generateRandom(minZ, maxZ));
    } 
    
    private static float generateRandom(float min, float max)
    {
        Random random = new Random();
        return (float)((random.NextDouble() * (max - min)) + min);
    }
}
