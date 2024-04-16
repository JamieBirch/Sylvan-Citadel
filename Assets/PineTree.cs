using UnityEngine;

public class PineTree : Tree
{
    public int Fertility;
    public GameObject pinePrefab;
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
    }
    
    void StartDay()
    {
        switch (treeSize.GetSize())
        {
            /*case < sizeSmall:
            {
                break;
            }*/
            case < sizeMiddle:
            {
                double chance = Utils.GenerateRandomChance();
                if (chance <= Fertility)
                {
                    // Debug.Log("I feel fruity today!");
                    DropPine();
                }
                break;
            }
            case < sizeOld*2:
            {
                double chance = Utils.GenerateRandomChance();
                if (chance <= Fertility)
                {
                    // Debug.Log("I feel very fruity today!");
                    DropPine();
                    DropPine();
                }
                break;
            }
            default:
            {
                Debug.Log("Unknown Tree size condition");
                break;
            }
        }
    }

    private void DropPine()
    {
        GameObject _pine = Instantiate(pinePrefab, transform.position + PinePositionOffset(), Quaternion.identity);
        Pine pineComponent = _pine.GetComponent<Pine>();
        pineComponent.hex = hex;
    }

    private static Vector3 PinePositionOffset()
    {
        return new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0f, Utils.GenerateRandom(-0.5f, 0.5f));
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
