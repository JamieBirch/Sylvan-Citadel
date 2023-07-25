using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject fruit;
    public float size;
    public float growthSpeed;

    public int Fertility;

    private void Start()
    {
        size = gameObject.transform.localScale.magnitude * 10;
        GameManager.NewDay += StartDay;
    }
    
    void StartDay()
    {
        gameObject.transform.localScale *= growthSpeed;
        size = gameObject.transform.localScale.magnitude * 10;

        if (size > 12)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance <= Fertility)
            {
                // Debug.Log("I feel fruity today!");
                Instantiate(fruit, transform.position + fruitPositionOffset(), Quaternion.identity, gameObject.GetComponentInParent<Woodland>().transform);
                GameStats.FruitsAvailable++;
            }
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
