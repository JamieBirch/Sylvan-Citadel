using UnityEngine;

public class Tree : MonoBehaviour
{
    public OwnedHex hex;
    public GameObject fruitPrefab;
    public GameObject leaves;
    public float size;
    public float growthSpeed;

    public int Fertility;

    public const int sizeOvergrown = 60;
    public const int sizeSmall = 12;
    public const int sizeMiddle = 30;
    public const int sizeOld = 50;
    private void Start()
    {
        size = gameObject.transform.localScale.magnitude * 10;
        Calendar.NewDay += StartDay;
    }
    
    void StartDay()
    {
        if (size < sizeOvergrown)
        {
            gameObject.transform.localScale *= growthSpeed;
            size = gameObject.transform.localScale.magnitude * 10;
        }

        switch (size)
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
                    Debug.Log("I feel very fruity today!");
                    BearFruit();
                    BearFruit();
                }
                break;
            }
            default:
            {
                leaves.SetActive(false);
                Debug.Log("I don't bear fruits anymore!");
                break;
            }
        }
    }

    private void BearFruit()
    {
        Woodland _woodland = gameObject.GetComponentInParent<Woodland>();
        GameObject _fruit = Instantiate(fruitPrefab, transform.position + fruitPositionOffset(), Quaternion.identity,
            _woodland.transform);

        Fruit fruitComponent = _fruit.GetComponent<Fruit>();
        fruitComponent.woodland = _woodland;
        fruitComponent.hex = hex;
        hex.FruitsAvailable++;
    }

    private Vector3 fruitPositionOffset()
    {
        // Vector3 offset = new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0.5f, Utils.GenerateRandom(-0.5f, 0.5f));
        Vector3 offset = new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0f, Utils.GenerateRandom(-0.5f, 0.5f));
        return offset;
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
