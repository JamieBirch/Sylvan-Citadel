using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO: move smoother
    //TODO: screen margin camera movement
    //TODO: camera rotation
    public float panSpeed;
    public float panBorderThickness;
    public Vector2 panLimit;

    public float scrollSpeed;
    public float miny = 20f;
    public float maxy = 120f;
    private float velocity = 0;

    void Update()
    {
        Vector3 pos = transform.position;
        
        //move
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = (Mathf.Clamp(pos.x, -panLimit.x, panLimit.x));
        pos.z = (Mathf.Clamp(pos.z, -panLimit.y, panLimit.y));

        //zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y = Mathf.SmoothDamp(pos.y, pos.y - scroll * scrollSpeed * 100f * Time.deltaTime, ref velocity, Time.deltaTime);
        // Vector3 targetPosition = target.position + offset;
        // pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        pos.y = (Mathf.Clamp(pos.y, miny, maxy));

        transform.position = Vector3.Lerp(pos, new Vector3(pos.x, pos.y, pos.z),  Mathf.SmoothStep(0, 1,Time.deltaTime / 100f));
    }
}
