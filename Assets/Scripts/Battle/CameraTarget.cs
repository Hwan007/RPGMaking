using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Terrain terrainBase;
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

    void Start()
    {
        velocity = Vector3.zero;
        mainCamera = transform.Find("Main Camera").GetComponent<MainCamera>();
        terrainBase = GameObject.Find("Terrain/Terrain").GetComponent<Terrain>();
    }

    void Update()
    {
        // Get the height of the terrain at the player's position
        float playerHeight = terrainBase.SampleHeight(transform.position);

        // Set the player's position to be on top of the terrain
        // if (transform.position.y < playerHeight + transform.localScale.y * 0.5f)
        transform.position = new Vector3(transform.position.x, playerHeight + transform.localScale.y * 0.5f, transform.position.z);

        RotateDependsOnMouse();
        MovePlayer();
    }

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
}
/*
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
*/