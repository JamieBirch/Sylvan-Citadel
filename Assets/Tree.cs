using UnityEngine;

using Random = System.Random;


public class Tree : MonoBehaviour
{
    public GameObject fruit;

    public static int Fertility = 20;
    public Vector3 offset = new Vector3(0.05f, 0.1f, 0.05f);

    private void Start()
    {
        GameManager.NewDay += StartDay;
    }
    
    void StartDay()
    {
        //TODO spawn fruit with 80% chance
        double chance = new Random().NextDouble() * 100;
        if (chance <= Fertility)
        {
            Debug.Log("I feel fruity today!");
            Instantiate(fruit, transform.position + offset, Quaternion.identity);
            GameStats.FruitsAvailable++;
        }
    }
    
    public void OnDestroy()
    {
        GameManager.NewDay -= StartDay;
    }
}
