using System.Collections;
using UnityEngine;

namespace ProjectCode
{
    public class MeleeWeapon : Weapon.WeaponTypeStat
    {
        public class PartStat
        {
            public int MaterialLevel;
            public Item Material;
            public float Length;
        }

        public enum BladeTypeList
        {
            Axe,
            Sword,
            Spear
        }

        public PartStat Blade = new PartStat { MaterialLevel = 0, Length = 1f, Material = null };
        public PartStat Handle = new PartStat { MaterialLevel = 0, Length = 1f, Material = null };
        public BladeTypeList BladeType = 0;

        public void InitWeaponStat()
        {
            base.Class = Weapon.WeaponClass.Melee;

        }
    }
}