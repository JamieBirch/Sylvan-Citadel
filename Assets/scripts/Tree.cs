using UnityEngine;

public class Tree : MonoBehaviour
{
    public OwnedHex hex;
    public GameObject leaves;
    public TreeSize treeSize;
    public float growthSpeed;

    public const int sizeSmall = 4;
    public const int sizeMiddle = 10;
    public const int sizeOld = 15;
    public const int sizeOvergrown = 20;

    private void Start()
    {
        Calendar.NewDay += StartDay;
    }
    
    void StartDay()
    {
        if (treeSize.GetSize() < sizeOvergrown)
        {
            treeSize.Grow(growthSpeed);
        }
        else
        {
            leaves.SetActive(false);
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
