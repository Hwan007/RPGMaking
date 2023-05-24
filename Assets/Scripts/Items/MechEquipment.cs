using System.Collections.Generic;
using UnityEngine;
using ProjectCode;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    [CreateAssetMenu(fileName = "Mech Equipment", menuName = "Item/Mech Equipment", order = -999)]
    public class MechEquipment : Item
    {
        public MechEquipment()
        {
            
        }
        public Sprite[] Sprite = new Sprite[2];
        public enum MechSlot
        {
            head,
            torso,
            arm,
            leg,
            left_weapon,
            right_weapon,
            Core
        }
        /*
        [System.Serializable]
        public class BaseStat
        {
            public int Health;
            public int Armour;
        }

        [Header("Stats")]
        public BaseStat Stats = new BaseStat()
        {
            Health = 1,
            Armour = 1
        };
        */
        public abstract class Slot : ScriptableObject
        {
            public string Description;
            public MechSlot PartSlot;
            public virtual MechSlot SlotInfo()
            {
                return PartSlot;
            }
            public abstract void EquippedSlot(CharacterData user);
            public abstract void RemovedSlot(CharacterData user);
            public virtual string GetDescription()
            {
                return Description;
            }
        }

        public Slot SlotStat;

        public abstract class Effect : ScriptableObject
        {
            public string EffectName;
            public string Description;
            public abstract void EquippedEffect(CharacterData user);
            public abstract void RemovedEffect(CharacterData user);

            public virtual string GetDescription()
            {
                return EffectName + "\n" + Description;
            }
        }

        public List<Effect> EquippedEffects;

        public override string GetDescription()
        {
            string desc = base.GetDescription();

            if(SlotStat.GetDescription() != null)
                desc += "\n" + SlotStat.GetDescription();
            foreach (var effect in EquippedEffects)
                desc += "\n" + effect.GetDescription();
            return desc;
        }

        public void EquippedBy(CharacterData user)
        {
            foreach (var effect in EquippedEffects)
                effect.EquippedEffect(user);
        }

        public void UnequippedBy(CharacterData user)
        {
            foreach (var effect in EquippedEffects)
                effect.RemovedEffect(user);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MechEquipment))]
public class EquipmentItemEditor : Editor
{
    MechEquipment m_Target;

    ItemEditor m_ItemEditor;

    SerializedProperty m_SpriteProperty;

    List<string> m_AvailableEquipEffectType;
    SerializedProperty m_EquippedEffectListProperty;

    List<string> m_AvailableSlotType;
    SerializedProperty m_SlotProperty;

    public int SelectPart = -1;
    public int SelectedPart = -1;

    void OnEnable()
    {
        m_Target = target as MechEquipment;

        m_EquippedEffectListProperty = serializedObject.FindProperty(nameof(MechEquipment.EquippedEffects));
        m_SlotProperty = serializedObject.FindProperty(nameof(MechEquipment.SlotStat));
        m_SpriteProperty = serializedObject.FindProperty(nameof(MechEquipment.Sprite));

        m_ItemEditor = new ItemEditor();
        m_ItemEditor.Init(serializedObject);

        var lookup = typeof(MechEquipment.Effect);
        m_AvailableEquipEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();

        lookup = typeof(MechEquipment.Slot);
        m_AvailableSlotType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        m_ItemEditor.GUI();
        EditorGUILayout.PropertyField(m_SpriteProperty);
        EditorGUILayout.LabelField("1: Face side    2: Back side", EditorStyles.toolbar);
        /*
        var child = m_BaseStatProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField(" Base Stat", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }
        */
        EditorGUILayout.LabelField("Base Stat", EditorStyles.boldLabel);
        SelectPart = EditorGUILayout.Popup("Add Stat", SelectedPart, m_AvailableSlotType.ToArray());
        if (SelectedPart != SelectPart)
        {
            var item = m_SlotProperty.objectReferenceValue;
            DestroyImmediate(item, true);

            var newInstance = ScriptableObject.CreateInstance(m_AvailableSlotType[SelectPart]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_SlotProperty.objectReferenceValue = newInstance;

            var child = m_SlotProperty.Copy();
            var depth = child.depth;
            child.NextVisible(true);

            while (child.depth > depth)
            {
                EditorGUILayout.PropertyField(child, true);
                child.NextVisible(false);
            }
        }
        SelectedPart = SelectPart;

        Editor ed = null;
        if(m_SlotProperty.objectReferenceValue != null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_SlotProperty;

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        /////////////////////////////////////

        int choice = EditorGUILayout.Popup("Add new Effect", -1, m_AvailableEquipEffectType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_AvailableEquipEffectType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_EquippedEffectListProperty.InsertArrayElementAtIndex(m_EquippedEffectListProperty.arraySize);
            m_EquippedEffectListProperty.GetArrayElementAtIndex(m_EquippedEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }

        ed = null;
        int toDelete = -1;
        for (int i = 0; i < m_EquippedEffectListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_EquippedEffectListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = m_EquippedEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            //need to do it twice, first time just nullify the entry, second actually remove it.
            m_EquippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif