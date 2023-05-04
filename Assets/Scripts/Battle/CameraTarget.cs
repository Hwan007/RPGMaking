using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Terrain terrainBase;
#if TestCode20230504
    public float normalSpeed = 20f;
    public float boostSpeed = 50f;
    public float rotateSpeed = 5f;
    public float acceleration = 80f;
    //public float deceleration = 50f;
    public float stopTime = 0.105f;
    float stopDeltaTime = 0.1f;
    float maxSpeed;
    public float verticalRotateUpperLimit = 40.0f;
    public float verticalRotateLowerLimit = 10.0f;

    MainCamera mainCamera;

    private Vector3 velocity;
#else
    public float NormalLimitSpeed = 30.0f;
    public float BoostLimitSpeed = 30.0f;
    public float BoostSpeedAmp = 3.0f;
    [Header("Power value must over Drag")]
    public float Power = 3.0f;
    [Header("Drag value must over 0")]
    public float Drag = 2.0f;
    [Header("Rotate Speed value must be between 0, 2")]
    public float RotateSpeed = 1.0f;
    int rotation = 0;
    Vector3 speed = Vector3.zero;
#endif
    void Start()
    {
#if TestCode20230504
        velocity = Vector3.zero;
        mainCamera = transform.Find("Main Camera").GetComponent<MainCamera>();
#endif
        terrainBase = GameObject.Find("Terrain").GetComponent<Terrain>();
        PreQuaternion = transform.rotation;
        DesireQuaternion = transform.rotation;
        speed = Vector3.zero;
    }

    void Update()
    {
        // Get the height of the terrain at the player's position
        float playerHeight = terrainBase.SampleHeight(transform.position);

        // Set the player's position to be on top of the terrain
        // if (transform.position.y < playerHeight + transform.localScale.y * 0.5f)
        transform.position = new Vector3(transform.position.x, playerHeight + transform.localScale.y * 0.5f, transform.position.z);
#if TestCode20230504
        RotateDependsOnMouse();
#endif
        MovePlayer();
        QERotate();
    }
#if TestCode20230504
    void RotateDependsOnMouse()
    {
        if (Input.GetMouseButton(1))
        {
            // Rotate the object based on the mouse input
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
            transform.Rotate(Vector3.up, mouseX, Space.World);
            //transform.Rotate(Vector3.right, -mouseY, Space.Self);

            transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x -mouseY, verticalRotateLowerLimit, verticalRotateUpperLimit)
                , transform.eulerAngles.y
                , transform.eulerAngles.z);
        }
    }
#endif
#if TestCode20230504
    void MovePlayer()
    {
        // Get the horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the camera view
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 direction = (forward * vertical + right * horizontal).normalized;

        // Set the speed limit based on whether the shift key is pressed
        float speedLimit = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : normalSpeed;
        speedLimit = speedLimit * (mainCamera.position + 1);
        
        // Calculate the target velocity based on the input and maximum speed
        Vector3 targetVelocity = direction * speedLimit;

        // Apply acceleration and deceleration to the velocity
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            float accel = acceleration * (mainCamera.distance[mainCamera.position]/5f);
            velocity = Vector3.MoveTowards(velocity, targetVelocity, accel * Time.deltaTime);
            /*
            if (Vector3.Dot(targetVelocity, velocity) > 0)
            {
                velocity = Vector3.MoveTowards(velocity, targetVelocity, accel * Time.deltaTime);
            }
            else
            {
                velocity = Vector3.MoveTowards(velocity, targetVelocity, decel * Time.deltaTime);
            }
            */
            if (stopDeltaTime > 0.1f)
                stopDeltaTime = 0.1f;
        }
        else
        {
            /*
            float decel = deceleration * (mainCamera.distance[mainCamera.position]/5f);
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decel * Time.deltaTime);
            */
            if (stopDeltaTime <= 0.1f)
            {
                maxSpeed = velocity.magnitude;
            }
            stopDeltaTime += Time.deltaTime;
            float decel = 2 * 100 / 16 * maxSpeed * (stopDeltaTime - stopTime);
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decel * Time.deltaTime);
        }

        // Move the object based on the current velocity
        transform.position += velocity * Time.deltaTime;
    }
#else
    
    void MovePlayer()
    {
        // Calculate the movement direction based on the camera view
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Get the horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (forward * vertical + right * horizontal).normalized;
        Vector3 Force;
        // Apply acceleration and deceleration to the velocity
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                Force = (direction * Power * BoostSpeedAmp - speed.normalized * Drag);
            else
                Force = (direction * Power - speed.normalized * Drag);

            var LimitSpeed = Input.GetKey(KeyCode.LeftShift) ? BoostLimitSpeed : NormalLimitSpeed;
            var cal = speed + Force * Time.deltaTime;
            if (cal.magnitude > LimitSpeed)
            {
                speed = cal.normalized * LimitSpeed;
            }
            else
            {
                speed = cal;
            }
        }
        else if (speed.magnitude > 0.01f)
        {
            Force = (- speed.normalized * Drag * 10.0f);
            speed += Force * Time.deltaTime;
        }
        else
        {
            Force = Vector3.zero;
            speed = Vector3.zero;
        }

        transform.position += speed * Time.deltaTime;
    }

    Quaternion PreQuaternion;
    Quaternion DesireQuaternion;
    float TimeCount = 0.0f;
    void QERotate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotation++;
            DesireQuaternion = Quaternion.Euler(PreQuaternion.eulerAngles + rotation * new Vector3(0, 90.0f, 0));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rotation--;
            DesireQuaternion = Quaternion.Euler(PreQuaternion.eulerAngles + rotation * new Vector3(0, 90.0f, 0));
        }

        if (Quaternion.Angle(transform.rotation, DesireQuaternion) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, DesireQuaternion, TimeCount / 2.0f * RotateSpeed);
            TimeCount += Time.deltaTime;
        }
        else if(rotation != 0)
        {
            transform.rotation = DesireQuaternion;
            PreQuaternion = transform.rotation;
            TimeCount = 0.0f;
            rotation = 0;
        }
    }
#endif
}
#if TestCode
public class CameraTarget : MonoBehaviour
{
    public Terrain terrainbase;

    Vector3 velocity;          // The current velocity

    void Start()
    {
        velocity = Vector3.zero;
     }
    void Update()
    {
        float height = terrainbase.SampleHeight(transform.position) + 5f;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        RotateDepandsOnMouse();
        KeyboardMovement2();
    }

    void RotateDepandsOnMouse()
    {
        float rotateSpeed = 5f;  // Rotation speed

        if (Input.GetMouseButton(1))
        {
            // Rotate the object based on the mouse input
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.Self);
        }
    }

    void KeyboardMovement2()
    {
        float normalSpeed = 10f;        // The maximum movement speed
        float boostSpeed = 30f;         // The maximum movement speed
        float acceleration;             // The acceleration rate

        // Get the horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the camera view
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 direction = (forward * vertical + right * horizontal).normalized;
        Debug.Log(direction);

        float speedLimit = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : normalSpeed;
        // Calculate the target velocity based on the input and maximum speed
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            acceleration = 100f;
        }
        else
        {
            acceleration = -150f;
        }
        
        velocity = direction * Mathf.Clamp(velocity.magnitude + acceleration * Time.deltaTime, 0, speedLimit);


        // Move the object based on the current velocity
        transform.position += velocity * Time.deltaTime;
    }
}
#endif