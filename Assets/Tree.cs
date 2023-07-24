using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject fruit;

    public int Fertility;

    private void Start()
    {
        GameManager.NewDay += StartDay;
    }
    
    void StartDay()
    {
        double chance = Utils.GenerateRandomChance();
        if (chance <= Fertility)
        {
            // Debug.Log("I feel fruity today!");
            Instantiate(fruit, transform.position + fruitPositionOffset(), Quaternion.identity);
            GameStats.FruitsAvailable++;
        }
    }

    private Vector3 fruitPositionOffset()
    {
        Vector3 offset = new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0.5f, Utils.GenerateRandom(-0.5f, 0.5f));
        return offset;
    }
    
    public void OnDestroy()
    {
        GameManager.NewDay -= StartDay;
    }
}
