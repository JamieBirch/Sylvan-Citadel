using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    public OwnedTile tile;
    public GameObject leaves;
    public TreeSize treeSize;
    public float growthSpeed;

    // public const int sizeYoungThreshhold = 4;
    public const int sizeMatureThreshhold = 4;
    public const int sizeOldThreshhold = 15;
    public const int sizeDeadThreshhold = 20;
    
    public int fertility;
    public GameObject seedPrefab;

    public abstract void DropSeed();
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
    }
    
    void StartDay()
    {
        switch (treeSize.GetSize())
        {
            //young tree
            case < sizeMatureThreshhold:
            {
                treeSize.Grow(growthSpeed);
                break;
            }
            //mature tree
            case < sizeOldThreshhold:
            {
                treeSize.Grow(growthSpeed);
                double chance = Utils.GenerateRandomChance();
                if (chance <= fertility)
                {
                    // Debug.Log("I feel fruity today!");
                    DropSeed();
                }
                break;
            }
            //old tree
            case < sizeDeadThreshhold:
            {
                treeSize.Grow(growthSpeed);
                double chance = Utils.GenerateRandomChance();
                if (chance <= fertility)
                {
                    // Debug.Log("I feel very fruity today!");
                    DropSeed();
                    DropSeed();
                }
                break;
            }
            case >= sizeDeadThreshhold:
            {
                leaves.SetActive(false);
                break;
            }
            default:
            {
                Debug.Log("Unknown Fruit Tree size condition");
                break;
            }
        }
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }

    public void Chop()
    {
        Destroy(gameObject);
    }
}
