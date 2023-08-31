using UnityEngine;

public class TreeSize : MonoBehaviour
{
    private float size;

    public float GetSize()
    {
        return size;
    }

    private void Start()
    {
        size = gameObject.transform.localScale.magnitude * 10;
    }

    public void Grow(float growthSpeed)
    {
        gameObject.transform.localScale *= growthSpeed;
        size = gameObject.transform.localScale.magnitude * 10;
    }
}
