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
    }
}