using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

public class SpawnUnit : MonoBehaviour
{
    private void Awake()
    {
        Transform T_Ally = transform.GetChild(0);
        Transform T_Enemy = transform.GetChild(1);
        if (SaveUnitList.UnitList != null)
        {
            if (SaveUnitList.UnitList.Ally.Count != 0)
            {
                foreach (CharacterData unit in SaveUnitList.UnitList.Ally)
                {

                }
            }

            if (SaveUnitList.UnitList.Enemy.Count != 0)
            {
                foreach (CharacterData unit in SaveUnitList.UnitList.Enemy)
                {

                }
            }
        }
    }
    
    void Start()
    {
        
    }

    void MakeUnit()
    {

    }
}
