using System.Collections;
using UnityEngine;

namespace ProjectCode
{
    public class MeleeWeapon : Weapon.Property
    {
        public class PartStat
        {
            public Material Material;
            public int Length;
            public int DefaultLength;
        }

        public enum BladeTypeList
        {
            Axe,
            Sword,
            Spear
        }

        public struct BladeProperty
        {
            public int MinimumLength;
            public int MaximumLength;
            public int BaseDamage;
            public int BaseAccuracy;
        }

        public BladeProperty Axe;
        public BladeProperty Sword;
        public BladeProperty Spear;

        public PartStat Blade = new PartStat { DefaultLength = 1, Length = 1, Material = null };
        public PartStat Handle = new PartStat { DefaultLength = 1, Length = 1, Material = null };
        public BladeTypeList BladeType = 0;

        public void InitWeaponStat()
        {
            base.Class = Weapon.WeaponClass.Melee;
            Blade = new PartStat { DefaultLength = 1, Length = 1, Material = null };
            Handle = new PartStat { DefaultLength = 1, Length = 1, Material = null };
        }

        public int SetMaterial(string part, Item material)
        {
            int ret = 0;
            if (part == "Blade")
            {
                Blade.Material = material as Material;
            }
            else if (part == "Handle")
            {
                Handle.Material = material as Material;
            }
            else
            {
                Debug.Log("Wrong part name (" + part + ")");
                ret = -1;
            }
            return ret;
        }

        public int SetProperty(string property, int input)
        {
            int ret = 0;
            if (property == "Length")
            {

            }
            else if (property == "Blade Type")
            {

            }
            else if (property == "Gear")
            {

            }
            else
            {
                ret = -1;
            }
            return ret;
        }
    }
}