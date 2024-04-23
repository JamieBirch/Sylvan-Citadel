using UnityEngine;

public class Lake : MonoBehaviour
{
    public float size;
    
    // Start is called before the first frame update
    void Start()
    {
        size = gameObject.transform.localScale.magnitude * 10;
    }
    
}
