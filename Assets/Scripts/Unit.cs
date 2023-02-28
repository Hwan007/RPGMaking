using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Terrain terrainBase;
    public Quaternion normalDirection;
    public Quaternion towardDirection;
    // Start is called before the first frame update
    void Start()
    {
        normalDirection = transform.Find("head").rotation;
        terrainBase = GameObject.Find("Terrain").GetComponentInChildren<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        towardDirection = transform.rotation;
        normalDirection = transform.Find("head").rotation;
        float height = terrainBase.SampleHeight(transform.position);
        if (transform.position.y <= height + transform.localScale.y * 0.5f)
        {
            transform.position = new Vector3(transform.position.x, height + transform.localScale.y * 0.5f, transform.position.z);
        }
    }

    public void ClickHighlight()
    {
        transform.Find("highlight").gameObject.SetActive(true);
    }
}
