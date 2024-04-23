using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO: move smoother
    //TODO: screen margin camera movement
    //TODO: camera rotation
    public float panSpeed;
    public Vector2 panLimit;

    public float scrollSpeed;
    public float miny = 20f;
    public float maxy = 120f;

    void Update()
    {
        Vector3 pos = transform.position;
        
        //move
        if (Input.GetKey("w"))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = (Mathf.Clamp(pos.x, -panLimit.x, panLimit.x));
        pos.z = (Mathf.Clamp(pos.z, -panLimit.y, panLimit.y));

        //zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        pos.y = (Mathf.Clamp(pos.y, miny, maxy));

        transform.position = Vector3.Lerp(pos, new Vector3(pos.x, pos.y, pos.z),  Mathf.SmoothStep(0, 1,Time.deltaTime / 100f));
    }
}
