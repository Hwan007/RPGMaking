using System.Collections;
using UnityEngine;
using ProjectCode;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public class Melee : Weapon.Property
    {
        [System.Serializable]
        public class PartStat
        {
            public Material Material;
            public int Length;
            public int DefaultLength;
        }
        public PartStat Blade = new PartStat { DefaultLength = 1, Length = 1, Material = null };
        public PartStat Handle = new PartStat { DefaultLength = 1, Length = 1, Material = null };

        public enum BladeTypeList
        {
            Axe,
            Sword,
            Spear
        }
        public BladeTypeList BladeType;
        
        [System.Serializable]
        public struct BladeProperty
        {
            public int MinimumLength;
            public int MaximumLength;
            public int BaseDamage;
            public int BaseAccuracy;
            public int MinimumDamage;
            public int MaximumDamage;
            public float BaseDuration;
            public float PreDelay;
            public float PostDelay;
        }
        
        public BladeProperty Axe;
        public BladeProperty Sword;
        public BladeProperty Spear;

        [System.Serializable]
        public struct GearProperty
        {
            public int Penalty;
            public int FireDamage;
            public int ShockDamage;
            public float ShieldPierceProbability;
            public bool ColdDamage;
            public bool LightningDamage;
        }
        public GearProperty Gear;

        public void InitWeaponStat()
        {
            base.Class = Weapon.WeaponType.Melee;
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

        public override Weapon.WeaponBaseStat Calculating()
        {
            Weapon.WeaponBaseStat ret;
            Weapon.WeaponBaseStat original = base.WeaponStats;

            BladeProperty select;
            switch (BladeType)
            {
                case BladeTypeList.Axe:
                    select = Axe;
                    break;
                case BladeTypeList.Spear:
                    select = Spear;
                    break;
                case BladeTypeList.Sword:
                    select = Sword;
                    break;
                default:
                    select = new BladeProperty() { MinimumLength = -1, MaximumLength = -1, BaseAccuracy = -1, BaseDamage = -1 };
                    break;
            }

            if (select.BaseDamage == -1)
            {
                Debug.Log("Wrong Blade Type");
                return original;
            }    

            ret.Weight = Mathf.FloorToInt(Blade.Length * Blade.Material.Density + Handle.Length * Handle.Material.Density);
            
            if (select.MinimumLength > Blade.Length || select.MaximumLength < Blade.Length)
            {
                Debug.Log("Wrong Blade Length : (" + Blade.Length + ")");
                return original;
            }
            ret.Damage = Mathf.FloorToInt(( Blade.Material.Level * Blade.Material.DamageCorrection + select.BaseDamage ) * ( Blade.Length / Blade.DefaultLength ));
            ret.FireDamage = Gear.FireDamage;
            ret.ShockDamage = Gear.ShockDamage;
            ret.ColdDamage = Gear.ColdDamage;
            ret.LightningDamage = Gear.LightningDamage;

            if (Handle.Length > Handle.DefaultLength * 4 || Handle.Length < Handle.DefaultLength / 4)
            {
                Debug.Log("Wrong Handle Length : (" + Handle.Length + ")");
                return original;
            }
            ret.Accuracy = ( Handle.Material.Level * Handle.Material.AccuracyCorrection + select.BaseAccuracy ) * ( Handle.Length / Handle.DefaultLength );
            float WeaponRange;
            if (base.Class == Weapon.WeaponType.Melee)
            {
                if (Handle.Length >= Handle.DefaultLength * 2)
                    WeaponRange = 2;
                else
                    WeaponRange = 1;
            }
            else
            {
                Debug.Log("Wrong Weapon Type");
                return original;
            }
            ret.Range = WeaponRange;
            ret.CriticalProbability = Mathf.Clamp(ret.Accuracy - 100.0f, 0, 100.0f);
            ret.CriticalMagnification = 2.0f;
            ret.ShieldPierceProbability = Gear.ShieldPierceProbability;

            ret.NumberOfAttacks = 1 + Mathf.FloorToInt(( ret.Weight / (Blade.DefaultLength * Blade.Material.Density + Handle.DefaultLength * Handle.Material.Density) ));
            ret.MinimumDamage = Mathf.FloorToInt((Blade.Material.Level * Blade.Material.DamageCorrection + select.MinimumDamage) * (Blade.Length / Blade.DefaultLength));
            ret.MaximumDamage = Mathf.FloorToInt((Blade.Material.Level * Blade.Material.DamageCorrection + select.MaximumDamage) * (Blade.Length / Blade.DefaultLength));
            ret.Duration = select.BaseDuration * ( ret.Weight /  ( Blade.DefaultLength * Blade.Material.Density + Handle.DefaultLength * Handle.Material.Density ) );
            ret.PreDelay = select.PreDelay * (ret.Weight / (Blade.DefaultLength * Blade.Material.Density + Handle.DefaultLength * Handle.Material.Density));
            ret.PostDelay = select.PostDelay * (ret.Weight / (Blade.DefaultLength * Blade.Material.Density + Handle.DefaultLength * Handle.Material.Density));

            base.WeaponStats = ret;
            Debug.Log("Weapon stats calculation complete");
            //Debug.Log(base.WeaponStats.Range);
            return ret;
        }
    }
}
/*
#if UNITY_EDITOR
[CustomEditor(typeof(Melee))]
public class MeleeEditor : Editor
{
    
}
#endif
*/