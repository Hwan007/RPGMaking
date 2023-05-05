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
    public class head  :MechEquipment.Slot
    {
        public StatSystem.StatModifier Modifier;
        [Header("Stat per Slot")]
        public int RequiredPower;
        [Header("Connected Part\n1:torso")]
        public Vector3[] HeadJoint = new Vector3[1];
        public override void EquippedSlot(CharacterData user)
        {
            user.Stats.AddModifier(Modifier);
        }
        public override void RemovedSlot(CharacterData user)
        {
            user.Stats.RemoveModifier(Modifier);
        }
        public override MechEquipment.MechSlot GetSlot()
        {
            base.PartSlot = MechEquipment.MechSlot.Head;
            return base.PartSlot;
        }
    }
}
