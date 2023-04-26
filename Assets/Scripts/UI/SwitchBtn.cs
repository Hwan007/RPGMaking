using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBtn : MonoBehaviour
{
    GameObject clickedObject;
    // Start is called before the first frame update
    void Start()
    {
        clickedObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        Debug.Log("Clicked " + clickedObject.name);
        if (clickedObject.name == "right")
        {
            Debug.Log(clickedObject.name + transform.parent.gameObject.name);
        }
        else if (clickedObject.name == "left")
        {
            Debug.Log(clickedObject.name + transform.parent.gameObject.name);
        }
    }
}
