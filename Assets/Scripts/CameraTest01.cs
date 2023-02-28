using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest01 : MonoBehaviour
{
    public float BoostSpeed = 20f;
    public float NormalSpeed = 10f;

    public float Power = 200f;
    public float Drag = 100f;
    Vector3 Velocity;

    public float StopDeltaTime = 0.3f;

    int ZoomLevel = 0;
    public float[] ZoomDistance = { 10f, 20f, 50f };

    public float rotateSpeed = 10f;
    public float verticalRotateLowerLimit = 30f;
    public float verticalRotateUpperLimit = 60f;

    Camera CameraComponent;

    bool Click;
    Vector3 ClickPosition;

    // Start is called before the first frame update
    void Start()
    {
        CameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotateDependsOnMouse();
        MoveToClickPoint();
        ZoomDependsOnMouse();

        Vector3 decelDir = - Velocity.normalized;
        float MaxSpeed = Input.GetKey(KeyCode.LeftShift) ? NormalSpeed : BoostSpeed;
        Velocity += decelDir * Drag * ( Velocity.magnitude) / MaxSpeed * Time.deltaTime;

        if (collide)
        {
            Velocity -= Vector3.Dot(Velocity, collideVector.normalized) * collideVector.normalized;
        }
        transform.position += Velocity * Time.deltaTime;
    }

    void MovePlayer()
    {
        if (Click) return;
        // Calculate the movement direction based on the camera view
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Apply acceleration and deceleration to the velocity
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            // Get the horizontal and vertical input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = (forward * vertical + right * horizontal).normalized;
            Velocity += direction * Power * Time.deltaTime;
        }
    }

    void RotateDependsOnMouse()
    {
        if (Click) return;
        if (Input.GetMouseButton(1))
        {

            // Rotate the object based on the mouse input
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

            Ray ray = CameraComponent.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                transform.RotateAround(hit.point, Vector3.up, mouseX);
            }

            transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x - mouseY, verticalRotateLowerLimit, verticalRotateUpperLimit)
                , transform.eulerAngles.y
                , transform.eulerAngles.z);
        }
    }

    Vector3 zoomVel;
    void ZoomDependsOnMouse()
    {
        int wheelData = Mathf.Clamp(-Mathf.RoundToInt(Input.mouseScrollDelta.y), -1, 1);
        ZoomLevel = Mathf.Clamp(ZoomLevel += wheelData, 0, ZoomDistance.Length - 1);

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, ZoomDistance[ZoomLevel], transform.position.z), ref zoomVel, 0.5f);
    }

    Vector3 refVel;
    void MoveToClickPoint()
    {
        if (DoubleClick())
        {
            RaycastHit hit;
            RaycastHit offset;
            Ray ray = CameraComponent.ScreenPointToRay(Input.mousePosition);
            Ray offsetRay = CameraComponent.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

            if (Physics.Raycast(ray, out hit))
            {
                Physics.Raycast(offsetRay, out offset);
                if (hit.collider.tag == "Unit")
                {
                    ClickPosition = hit.collider.transform.position + (offset.point - transform.position);
                }
                else
                {
                    ClickPosition = hit.point + (offset.point - transform.position);
                }
                Click = true;
            }
        }
        else if( Input.anyKeyDown && Input.GetMouseButtonUp(0) == false )
        {
            Click = false;
        }

        if(Click)
        {
            transform.position = Vector3.SmoothDamp(transform.position, ClickPosition, ref refVel, 2f);
            if (transform.position == ClickPosition - Vector3.one * 0.001f)
                Click = false;
        }
    }

    int Clicked = 0;
    float ClickTime = 0;
    public float ClickDelay = 0.5f;
    bool DoubleClick()
    {
        if ( Input.GetMouseButtonUp(0) )
        {
            if (Clicked == 0) ClickTime = Time.time;
            Clicked++;
        }
        if ( Clicked > 1 && Time.time - ClickTime < ClickDelay)
        {
            Clicked = 0;
            ClickTime = 0;
            return true;
        }
        else if ( Clicked > 2 || Time.time - ClickTime > ClickDelay)
        {
            Clicked = 0;
        }
        return false;
    }

    public Vector3 collideVector;
    public bool collide;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "terrain")
        {
            collide = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("collide");
        if (collide)
        {
            ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];

            int points = collision.GetContacts(contactPoints);

            if (points == 1)
            {
                collideVector = transform.position - contactPoints[0].point;
            }
            else
            {
                Vector3 middlePoint = Vector3.zero;
                for(int i=0; i < points; i++)
                {
                    middlePoint += contactPoints[i].point;
                }
                middlePoint = middlePoint / points;

                collideVector = transform.position - middlePoint;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "terrain")
        {
            collide = false;
        }
    }
}
