using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject fruit;

    public int Fertility;
    public Vector3 offset = new Vector3(0.05f, 0.1f, 0.05f);

    private void Start()
    {
        GameManager.NewDay += StartDay;
    }
    
    void StartDay()
    {
        double chance = Utils.GenerateRandomChance();
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
