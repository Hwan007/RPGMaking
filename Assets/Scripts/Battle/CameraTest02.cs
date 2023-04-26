using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest02 : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    public float power = 10f;
    public float drag = 1f;
    public float angularDrag = 0.5f;
    public float mass = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        m_Rigidbody.mass = mass;
        m_Rigidbody.drag = drag;
        m_Rigidbody.angularDrag = angularDrag;
        m_Rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }


    bool Click;
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
            m_Rigidbody.AddForce(direction * power);
        }
    }
}
