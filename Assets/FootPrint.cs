using UnityEngine;

public class FootPrint : MonoBehaviour
{
    public float countdown;
    
    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Destroy(gameObject);
        }
    }
}
