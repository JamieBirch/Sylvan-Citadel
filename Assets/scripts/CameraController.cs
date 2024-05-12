using UnityEngine;

public class CameraController : MonoBehaviour
{
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
            Vector3 target = transform.position + gameObject.transform.forward * (panSpeed * Time.deltaTime);
            pos.z = target.z;
            pos.x = target.x;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            Vector3 target = transform.position - gameObject.transform.forward * (panSpeed * Time.deltaTime);
            pos.z = target.z;
            pos.x = target.x;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos = transform.position + gameObject.transform.right * (panSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos = transform.position - gameObject.transform.right * (panSpeed * Time.deltaTime);
        }

        //zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if ((scroll > 0f && pos.y > miny) || (scroll < 0f && pos.y < maxy) )
        {
            Vector3 targetPosition = transform.position + gameObject.transform.forward * (scroll * scrollSpeed * 100f * Time.deltaTime);
            pos = targetPosition;
        }
        
        pos.x = (Mathf.Clamp(pos.x, -panLimit.x, panLimit.x));
        pos.z = (Mathf.Clamp(pos.z, -panLimit.y, panLimit.y));
        pos.y = (Mathf.Clamp(pos.y, miny, maxy));

        transform.position = pos;
    }
}
