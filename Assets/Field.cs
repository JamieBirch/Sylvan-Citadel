using UnityEngine;

public class Field : MonoBehaviour
{
    public float size;
    
    // Start is called before the first frame update
    void Start()
    {
        size = gameObject.transform.localScale.magnitude * 10;
    }
}
