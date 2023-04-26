using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameObject gameinfo;

    public class PartInfo
    {
        public int endurance;
        public int mass;
        public int inputPower;
        public string partName;
    }

    public class BodyPartInfo
    {
        public int endurance;
        public int mass;
        public int outputPower;
        public string partName;

        public class Circuit
        {
            public enum Type { none, circulation, shield, lightWeight, resist, magicWeapon };
            public int level;
        }
        public Circuit core1, core2, core3, core4, core5;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("gameinfo");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameinfo = gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            Debug.Log("Key input");
        }
    }
}
