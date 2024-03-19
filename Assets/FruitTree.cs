using UnityEngine;

public class FruitTree : Tree
{
    public int Fertility;
    public GameObject fruitPrefab;

private void Start()
    {
        Calendar.NewDay += StartDay;
    }
    
    void StartDay()
    {
        switch (treeSize.GetSize())
        {
            case < sizeSmall:
            {
                break;
            }
            case < sizeMiddle:
            {
                double chance = Utils.GenerateRandomChance();
                if (chance <= Fertility)
                {
                    // Debug.Log("I feel fruity today!");
                    BearFruit();
                }
                break;
            }
            case < sizeOld:
            {
                double chance = Utils.GenerateRandomChance();
                if (chance <= Fertility)
                {
                    // Debug.Log("I feel very fruity today!");
                    BearFruit();
                    BearFruit();
                }
                break;
            }
            default:
            {
                Debug.Log("Unknown Fruit Tree size condition");
                break;
            }
        }
    }

    private void BearFruit()
    {
        Woodland _woodland = gameObject.GetComponentInParent<Woodland>();
        GameObject _fruit = Instantiate(fruitPrefab, transform.position + FruitPositionOffset(), Quaternion.identity,
            _woodland.transform);

        Fruit fruitComponent = _fruit.GetComponent<Fruit>();
        fruitComponent.woodland = _woodland;
        fruitComponent.hex = hex;
    }

    private static Vector3 FruitPositionOffset()
    {
        return new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0f, Utils.GenerateRandom(-0.5f, 0.5f));
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
    
}
