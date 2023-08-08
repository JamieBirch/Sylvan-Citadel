using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public List<Human> humans;
    
    // Start is called before the first frame update
    void Start()
    {
        if (humans.Count == 0)
        {
            humans = new List<Human>();
        }
    }
}
