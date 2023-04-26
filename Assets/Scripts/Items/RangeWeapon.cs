using System.Collections;
using UnityEngine;

namespace ProjectCode
{
    public class RangeWeapon : Weapon.WeaponTypeStat
    {
        public enum AttackTypeList
        {
            Energy,
            Physical
        }

        public AttackTypeList AttackType;


    }
}