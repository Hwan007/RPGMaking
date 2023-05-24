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
    public class leg  :MechEquipment.Slot
    {
        public StatSystem.StatModifier Modifier;
        [Header("Stat per Slot")]
        public int RequiredPower;
        [Header("Connected Part\n1:torso")]
        public Vector3[] LegJoint = new Vector3[1];
        public override void EquippedSlot(CharacterData user)
        {
            user.Stats.AddModifier(Modifier);
        }
        public override void RemovedSlot(CharacterData user)
        {
            user.Stats.RemoveModifier(Modifier);
        }
        public override MechEquipment.MechSlot SlotInfo()
        {
            base.PartSlot = MechEquipment.MechSlot.leg;
            return base.PartSlot;
        }
    }
}
