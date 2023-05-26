using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

namespace ProjectCode
{
    public class SaveUnitList : MonoBehaviour
    {
        public static SaveUnitList UnitList;
        public List<CharacterData> Ally;
        public List<CharacterData> Enemy;
        private void Awake()
        {
            UnitList = this;
            DontDestroyOnLoad(this);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}