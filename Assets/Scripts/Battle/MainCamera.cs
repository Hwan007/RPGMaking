using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /*
    public float[] distance = { 10.0f, 20.0f, 30.0f, 50.0f, 70.0f, 100.0f, 200.0f};
    public int position;
    */
    //public Vector3 velocity;
    Terrain terrainBase;

    Camera main;

    [Header("Starting Local Position")]
    public Vector3 Position = new Vector3(0, 10.0f, -11.0f);
    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Camera>();
        transform.SetParent(GameObject.Find("Camera Target").transform);
        //position = 3;
        //transform.localPosition = new Vector3(0, 0, -distance[position]);
        transform.localPosition = Position;
        transform.LookAt(transform.parent);
        //velocity = Vector3.zero;

        terrainBase = GameObject.Find("Terrain").GetComponent<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Get the height of the terrain at the player's position
        float playerHeight = terrainBase.SampleHeight(transform.position);

        int wheelData = Mathf.Clamp(-Mathf.RoundToInt(Input.mouseScrollDelta.y), -1, 1);
        position = Mathf.Clamp(position += wheelData, 0, distance.Length - 1);
        Vector3 tagetPosition = new Vector3(0,0,-distance[position]);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, tagetPosition, ref velocity, 0.3f);
        */
    }

}
