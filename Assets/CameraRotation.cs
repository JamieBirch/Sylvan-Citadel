using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float panSpeed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.down, panSpeed * Time.deltaTime);
        }
        if (Input.GetKey("q"))
        {
            transform.Rotate(Vector3.up, panSpeed * Time.deltaTime);
        }
    }
}
