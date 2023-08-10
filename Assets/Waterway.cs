using System.Collections.Generic;
using UnityEngine;

public class Waterway : MonoBehaviour
{
    public List<GameObject> lakes;
    
    // Start is called before the first frame update
    void Start()
    {
        if (lakes.Count == 0)
        {
            lakes = new List<GameObject>();
        }
    }
}
