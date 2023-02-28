using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitparts : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion normalDirection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        normalDirection = transform.parent.GetComponent<Unit>().normalDirection;
        if (gameObject.name == "head")
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
            transform.rotation = normalDirection;
    }
}
