using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public class torso :MechEquipment.Slot
    {
        public StatSystem.StatModifier Modifier;
        [Header("Stat per Slot")]
        public int OutputPower;
        public int CoreSlot;
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
            base.PartSlot = MechEquipment.MechSlot.Torso;
            return base.PartSlot;
        }
    }
}
