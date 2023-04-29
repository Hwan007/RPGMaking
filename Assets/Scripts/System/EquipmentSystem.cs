using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public class EquipmentSystem
    {
        public Weapon LeftWeapon { get; private set; }
        public Weapon RightWeapon { get; private set; }

        public Action<MechEquipment> OnEquiped { get; set; }
        public Action<MechEquipment> OnUnequip { get; set; }

        CharacterData m_Owner;

        MechEquipment m_HeadSlot;
        MechEquipment m_TorsoSlot;
        MechEquipment m_LegsSlot;
        MechEquipment m_ArmsSlot;

        Weapon m_DefaultRightWeapon;
        Weapon m_DefaultLeftWeapon;

        public void Init(CharacterData owner)
        {
            m_Owner = owner;
        }
        public void InitWeapon(Weapon wep, CharacterData data)
        {
            m_DefaultRightWeapon = wep;
            m_DefaultLeftWeapon = wep;
        }
        public MechEquipment GetItem(MechEquipment.MechSlot slot)
        {
            switch (slot)
            {
                case MechEquipment.MechSlot.Head:
                    return m_HeadSlot;
                case MechEquipment.MechSlot.Torso:
                    return m_TorsoSlot;
                case MechEquipment.MechSlot.Leg:
                    return m_LegsSlot;
                case MechEquipment.MechSlot.Arm:
                    return m_ArmsSlot;
                case (MechEquipment.MechSlot)100:
                    return LeftWeapon as MechEquipment;
                case (MechEquipment.MechSlot)200:
                    return RightWeapon as MechEquipment;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Equip the given item for the given user. This won't check about requirement, this should be done by the
        /// inventory before calling equip!
        /// </summary>
        /// <param name="item">Which item to equip</param>
        public void Equip(MechEquipment item)
        {
            Unequip(item.SlotStat.PartSlot, true);

            OnEquiped?.Invoke(item);

            switch (item.SlotStat.PartSlot)
            {
                case MechEquipment.MechSlot.Head:
                    {
                        m_HeadSlot = item;
                        m_HeadSlot.EquippedBy(m_Owner);
                    }
                    break;
                case MechEquipment.MechSlot.Torso:
                    {
                        m_TorsoSlot = item;
                        m_TorsoSlot.EquippedBy(m_Owner);
                    }
                    break;
                case MechEquipment.MechSlot.Leg:
                    {
                        m_LegsSlot = item;
                        m_LegsSlot.EquippedBy(m_Owner);
                    }
                    break;
                case MechEquipment.MechSlot.Arm:
                    {
                        m_ArmsSlot = item;
                        m_ArmsSlot.EquippedBy(m_Owner);
                    }
                    break;
                case (MechEquipment.MechSlot)100:
                    {
                        LeftWeapon = item as Weapon;
                        LeftWeapon.EquippedBy(m_Owner);
                    }
                    break;
                case (MechEquipment.MechSlot)200:
                    {
                        RightWeapon = item as Weapon;
                        RightWeapon.EquippedBy(m_Owner);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Unequip the item in the given slot. isReplacement is used to tell the system if this unequip was called
        /// because we equipped something new in that slot or just unequip to empty slot. This is for the weapon : the
        /// weapon slot can't be empty, so if this is not a replacement, this will auto-requip the base weapon.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="isReplacement"></param>
        public void Unequip(MechEquipment.MechSlot slot, bool isReplacement = false)
        {
            switch (slot)
            {
                case MechEquipment.MechSlot.Head:
                    if (m_HeadSlot != null)
                    {
                        m_HeadSlot.UnequippedBy(m_Owner);
                        OnUnequip?.Invoke(m_HeadSlot);
                        m_HeadSlot = null;
                    }
                    break;
                case MechEquipment.MechSlot.Torso:
                    if (m_TorsoSlot != null)
                    {
                        m_TorsoSlot.UnequippedBy(m_Owner);
                        OnUnequip?.Invoke(m_TorsoSlot);
                        m_TorsoSlot = null;
                    }
                    break;
                case MechEquipment.MechSlot.Leg:
                    if (m_LegsSlot != null)
                    {
                        m_LegsSlot.UnequippedBy(m_Owner);
                        OnUnequip?.Invoke(m_LegsSlot);
                        m_LegsSlot = null;
                    }
                    break;
                case MechEquipment.MechSlot.Arm:
                    if (m_ArmsSlot != null)
                    {
                        m_ArmsSlot.UnequippedBy(m_Owner);
                        OnUnequip?.Invoke(m_ArmsSlot);
                        m_ArmsSlot = null;
                    }
                    break;
                case (MechEquipment.MechSlot)100:
                    if (LeftWeapon != null &&
                        (LeftWeapon != m_DefaultLeftWeapon || isReplacement))
                    {
                        LeftWeapon.UnequippedBy(m_Owner);

                        if (LeftWeapon != m_DefaultLeftWeapon)
                            Debug.Log("put it in the inventory");

                        OnUnequip?.Invoke(LeftWeapon);
                        LeftWeapon = null;

                        if (!isReplacement)
                            Equip(m_DefaultLeftWeapon);
                    }
                    break;
                case (MechEquipment.MechSlot)200:
                    if (RightWeapon != null &&
                        (RightWeapon != m_DefaultRightWeapon || isReplacement))
                    {
                        RightWeapon.UnequippedBy(m_Owner);

                        if (RightWeapon != m_DefaultRightWeapon)
                            Debug.Log("put it in the inventory");

                        OnUnequip?.Invoke(RightWeapon);
                        RightWeapon = null;

                        if (!isReplacement)
                            Equip(m_DefaultRightWeapon);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
