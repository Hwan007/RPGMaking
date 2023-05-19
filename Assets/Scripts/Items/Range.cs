﻿using System.Collections;
using UnityEngine;

namespace ProjectCode
{
    public class Range : Weapon.Property
    {
        public enum AttackTypeList
        {
            Energy,
            Physical
        }

        public AttackTypeList AttackType;

        public override Weapon.WeaponBaseStat Calculating()
        {
            return base.WeaponStats;
        }
    }
}