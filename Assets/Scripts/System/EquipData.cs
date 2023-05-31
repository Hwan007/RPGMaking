using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public class EquipData
    {
        public int index;
        public string CharacterName;
        public StatSystem Stats;
        public EquipmentSystem Equipment = new EquipmentSystem();

        public void SetUnit(string name, StatSystem stat, EquipmentSystem equip)
        {
            CharacterName = name;
            Stats.stats.Copy(stat.stats);
            Equipment.Copy(equip);
        }

        public void GetUnit(ref string getName, ref StatSystem getStat, ref EquipmentSystem getEquipment)
        {
            getName = CharacterName;
            getStat.stats.Copy(Stats.stats);
            getEquipment.Copy(Equipment);
        }
    }
}