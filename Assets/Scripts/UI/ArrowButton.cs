using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    GameObject arrowDirec;
    // Start is called before the first frame update
    void Start()
    {
        arrowDirec = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        if ( arrowDirec.name == "right")
        {
            GameManager.I.ArrowClicked(false, arrowDirec.name);
        }
        else if ( arrowDirec.name == "left" )
        {
            GameManager.I.ArrowClicked(true, arrowDirec.name);
        }
    }
}
