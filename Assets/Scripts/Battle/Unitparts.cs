using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitparts : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion normalDirection;
    Unit ScriptUnit;
    void Start()
    {
        ScriptUnit = transform.parent.GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        normalDirection = ScriptUnit.normalDirection;
        if (gameObject.name == "head")
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
            transform.rotation = normalDirection;
    }
}
