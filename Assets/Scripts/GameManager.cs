using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public Scene sceneState;
    private void Awake()
    {
        I = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sceneState = SceneManager.GetActiveScene();
        Debug.Log(sceneState.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrowClicked(bool direction, string name)
    {
        Debug.Log(direction + name);
    }

    GameObject selected;
    public void SelectObject(GameObject selectObject)
    {
        selected = selectObject;
    }
}
