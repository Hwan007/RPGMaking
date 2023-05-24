using System.Collections.Generic;
using UnityEngine;
using ProjectCode;
using Vector3 = UnityEngine.Vector3;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace ProjectCode
{
    /// <summary>
    /// Special case of EquipmentItem for weapon, as they have a whole attack system in addition. Like Equipment they
    /// can have minimum stats and equipped effect, but also have a list of WeaponAttackEffect that will have their
    /// OnAttack function called during a hit, and their OnPostAttack function called after all OnAttack of all effects
    /// are called.
    /// </summary>
    public class Weapon : MechEquipment
    {
        public Weapon()
        {

        }
        /// <summary>
        /// This class will store damage done to a target CharacterData by a source CharacterData. The function to add
        /// damage will take care of applied all the strength/boost of the source and remove defense/resistance of the
        /// target.
        ///
        /// The source can be null when its done by an non CharacterData source (elemental effect, environment etc.)
        /// </summary>
        public class AttackData
        {
            public CharacterData Target => m_Target;
            public CharacterData Source => m_Source;

            CharacterData m_Target;
            CharacterData m_Source;

            int[] m_Damages = new int[System.Enum.GetValues(typeof(StatSystem.DamageType)).Length];

            /// <summary>
            /// Build a new AttackData. All AttackData need a target, but source is optional. If source is null, the
            /// damage is assume to be from a non CharacterData source (elemental effect, environment) and no boost will
            /// be applied to damage (target defense is still taken in account).
            /// </summary>
            /// <param name="target"></param>
            /// <param name="source"></param>
            public AttackData(CharacterData target, CharacterData source = null)
            {
                m_Target = target;
                m_Source = source;
            }

            /// <summary>
            /// Add an amount of damage given in the given type. The source (if non null, see class documentation for
            /// info) boost will be applied and the target defense will be removed from that amount.
            /// </summary>
            /// <param name="damageType">The type of damage</param>
            /// <param name="amount">The amount of damage</param>
            /// <returns></returns>
            public int AddDamage(StatSystem.DamageType damageType, int amount)
            {
                int addedAmount = amount;

                //Physical damage are increase by 1% for each point of strength
                if (damageType == StatSystem.DamageType.Physical)
                {
                    //source cna be null when it's elemental or effect damage
                    if (m_Source != null)
                        addedAmount += Mathf.FloorToInt(addedAmount);

                    //each poitn of defense remove 1 damage, with a minimum of 1 damage
                    // addedAmount = Mathf.Max(addedAmount - m_Target.MechStats.stats.defense, 1);
                }

                //we then add boost per damage type. Not this is called elementalBoost, but physical can also be boosted
                if (m_Source != null)
                    addedAmount += addedAmount * Mathf.FloorToInt(m_Source.Stats.stats.elementalBoosts[(int)damageType] / 100.0f);

                //Then the elemental protection that is a percentage
                addedAmount -= addedAmount * Mathf.FloorToInt(m_Target.Stats.stats.elementalProtection[(int)damageType] / 100.0f);

                m_Damages[(int)damageType] += addedAmount;

                return addedAmount;
            }

            /// <summary>
            /// Return the current amount of damage of the given type stored in that AttackData. This is the *effective*
            /// amount of damage, boost and defense have already been applied.
            /// </summary>
            /// <param name="damageType">The type of damage</param>
            /// <returns>How much damage of that type is stored in that AttackData</returns>
            public int GetDamage(StatSystem.DamageType damageType)
            {
                return m_Damages[(int)damageType];
            }

            /// <summary>
            /// Return the total amount of damage across all type stored in that AttackData. This is *effective* damage,
            /// that mean all boost/defense was already applied.
            /// </summary>
            /// <returns>The total amount of damage across all type in that Attack Data</returns>
            public int GetFullDamage()
            {
                int totalDamage = 0;
                for (int i = 0; i < m_Damages.Length; ++i)
                {
                    totalDamage += m_Damages[i];
                }

                return totalDamage;
            }
        }

        public enum WeaponType
        {
            Melee,
            Range
        }
        [System.Serializable]
        public abstract class Property : ScriptableObject
        {
            public WeaponType Class;

            public WeaponBaseStat WeaponStats { get; set; }

            public abstract WeaponBaseStat Calculating();
        }
        public Property WeaponProperty;

        /// <summary>
        /// Base class of all effect you can add on a weapon to specialize it. See documentation on How to write a new
        /// Weapon Effect.
        /// </summary>
        public abstract class WeaponAttackEffect : ScriptableObject
        {
            public string Description;

            //return the amount of physical damage. If no change, just return physicalDamage passed as parameter
            public virtual void OnAttack(CharacterData target, CharacterData user, ref AttackData data) { }

            //called after all weapon effect where applied, allow to react to the total amount of damage applied
            public virtual void OnPostAttack(CharacterData target, CharacterData user, AttackData data) { }

            public virtual string GetDescription()
            {
                return Description;
            }
        }

        [System.Serializable]
        public struct WeaponBaseStat
        {
            // output information of weapon
            public int Weight;
            public int Damage;
            public int FireDamage;
            public int ShockDamage;
            public bool ColdDamage;
            public bool LightningDamage;
            public float Accuracy;
            public float Range;
            public float CriticalProbability;
            public float CriticalMagnification;
            public float ShieldPierceProbability;

            // value for using calculation
            public int NumberOfAttacks;
            public int MinimumDamage;
            public int MaximumDamage;
            public float Duration;
            public float PreDelay;
            public float PostDelay;
        }

        public WeaponBaseStat WeaponStats;

        public List<WeaponAttackEffect> AttackEffects;

        public void Attack(CharacterData attacker, CharacterData target)
        {
            AttackData attackData = new AttackData(target, attacker);

            int damage = Random.Range(WeaponStats.MinimumDamage, WeaponStats.MaximumDamage + 1) * WeaponStats.NumberOfAttacks;

            attackData.AddDamage(StatSystem.DamageType.Physical, damage);

            foreach (var wae in AttackEffects)
                wae.OnAttack(target, attacker, ref attackData);

            target.Damage(attackData);

            foreach (var wae in AttackEffects)
                wae.OnPostAttack(target, attacker, attackData);
        }
        public bool CanHit(CharacterData attacker, CharacterData target)
        {
            if (Vector3.SqrMagnitude(attacker.transform.position - target.transform.position) < WeaponStats.Range * WeaponStats.Range)
            {
                return true;
            }

            return false;
        }

        public override string GetDescription()
        {
            string desc = base.GetDescription();

            desc += "\n";
            desc += $"Damage: {WeaponStats.MinimumDamage * WeaponStats.NumberOfAttacks} - {WeaponStats.MaximumDamage * WeaponStats.NumberOfAttacks}\n";
            desc += $"Number of Attack : {WeaponStats.NumberOfAttacks}\n";
            desc += $"Pre Delay : {WeaponStats.PreDelay}\n";
            desc += $"Attack Duration : {WeaponStats.Duration}\n";
            desc += $"Post Delay : {WeaponStats.PostDelay}\n";
            desc += $"Range : {WeaponStats.Range}m\n";

            return desc;
        }

        [Header("Sounds")]
        public AudioClip[] HitSounds;
        public AudioClip[] FireSounds;
        
        public AudioClip GetHitSound()
        {
            if (HitSounds == null || HitSounds.Length == 0)
                return SFXManager.GetDefaultHit();

            return HitSounds[Random.Range(0, HitSounds.Length)];
        }

        public AudioClip GetFireSound()
        {
            if (FireSounds == null || FireSounds.Length == 0)
                return SFXManager.GetDefaultFireSound();

            return FireSounds[Random.Range(0, FireSounds.Length)];
        }
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    Weapon m_Target;

    ItemEditor m_ItemEditor;

    List<string> m_AvailableEquipEffectType;
    SerializedProperty m_EquippedEffectListProperty;

    List<string> m_AvailableWeaponAttackEffectType;
    SerializedProperty m_WeaponAttackEffectListProperty;

    SerializedProperty m_HitSoundProps;
    SerializedProperty m_FireSoundProps;

    SerializedProperty m_WeaponStatProperty;

    List<string> m_AvailableWeaponTypeList;
    SerializedProperty m_ComponentStat;

    Weapon.WeaponType sel;
    bool show0;
    bool show1;
    [MenuItem("Assets/Create/Item/Weapon", priority = -999)]
    static public void CreateWeapon()
    {
        var newWeapon = CreateInstance<Weapon>();
        // newWeapon.Slot = (MechEquipment.MechSlot)666;

        ProjectWindowUtil.CreateAsset(newWeapon, "weapon.asset");
    }

    void OnEnable()
    {
        m_Target = target as Weapon;

        m_WeaponStatProperty = serializedObject.FindProperty(nameof(Weapon.WeaponStats));

        m_HitSoundProps = serializedObject.FindProperty(nameof(Weapon.HitSounds));
        m_FireSoundProps = serializedObject.FindProperty(nameof(Weapon.FireSounds));

        m_ItemEditor = new ItemEditor();
        m_ItemEditor.Init(serializedObject);

        m_EquippedEffectListProperty = serializedObject.FindProperty(nameof(Weapon.EquippedEffects));
        var lookup = typeof(MechEquipment.Effect);
        m_AvailableEquipEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();

        m_WeaponAttackEffectListProperty = serializedObject.FindProperty(nameof(Weapon.AttackEffects));
        lookup = typeof(Weapon.WeaponAttackEffect);
        m_AvailableWeaponAttackEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();

        m_ComponentStat = serializedObject.FindProperty(nameof(Weapon.WeaponProperty));
        lookup = typeof(Weapon.Property);
        m_AvailableWeaponTypeList = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();

        if (serializedObject.FindProperty(nameof(Weapon.WeaponProperty.Class)) == null)
        {
            sel = (Weapon.WeaponType) 5;
        }
        else
        {
            sel = (Weapon.WeaponType)serializedObject.FindProperty(nameof(Weapon.WeaponProperty.Class)).enumValueIndex;
        }
    }

    public override void OnInspectorGUI()
    {
        m_ItemEditor.GUI();
#if true
        GUILayout.BeginHorizontal();
        show1 = EditorGUILayout.Foldout(show1, new GUIContent("Weapon Stats"));
        if (GUILayout.Button("Reload", GUILayout.MaxWidth(80.0f)))
        {
            Debug.Log("Reload");
            m_Target.WeaponStats = m_Target.WeaponProperty.Calculating();
            m_WeaponStatProperty = serializedObject.FindProperty(nameof(Weapon.WeaponStats));
            AssetDatabase.Refresh();

            Editor edi = this;
            edi.Repaint();
        }
        GUILayout.EndHorizontal();
        if (show1 == true)
        {
            //EditorGUILayout.BeginVertical();
            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_WeaponStatProperty, GUIContent.none, true);
            GUI.enabled = true;
            //EditorGUILayout.EndVertical();
        }
#else
        var child =  m_WeaponStatProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);
        
        EditorGUILayout.LabelField("Weapon Stats", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }
#endif
        if (m_ComponentStat.objectReferenceValue == null)
        {
            GUILayout.BeginHorizontal();
            sel = (Weapon.WeaponType)EditorGUILayout.EnumPopup(new GUIContent("Add Stats"), sel, GUILayout.MaxWidth(250.0f));
            if (GUILayout.Button("Add", GUILayout.MaxWidth(40.0f)))
            {
                //Debug.Log(sel + "Clicked");
                int i;
                for (i = 0; i < m_AvailableWeaponTypeList.Count; i++)
                {
                    if (m_AvailableWeaponTypeList[i].ToString() == sel.ToString())
                    {
                        Debug.Log(m_AvailableWeaponTypeList[i].ToString() + sel + "Clicked");
                        break;
                    }
                }
                var newinstance = ScriptableObject.CreateInstance(m_AvailableWeaponTypeList[i]);

                AssetDatabase.AddObjectToAsset(newinstance, target);

                m_ComponentStat.objectReferenceValue = newinstance;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(m_ComponentStat, new GUIContent("Component Stats"), true);
            EditorGUILayout.BeginVertical();
            var position = EditorGUILayout.GetControlRect();
            show0 = EditorGUI.Foldout(position, show0, new GUIContent("Component Stats"));
            if (show0 == true)
            {
                EditorGUI.indentLevel++;
                Editor edi = null;
                Editor.CreateCachedEditor(m_ComponentStat.objectReferenceValue, null, ref edi);
                edi.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();


            if (GUILayout.Button(("Delete"), GUILayout.MaxWidth(80.0f)))
            {
                DestroyImmediate(m_ComponentStat.objectReferenceValue, true);
            }
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.PropertyField(m_HitSoundProps, true);
        EditorGUILayout.PropertyField(m_FireSoundProps, true);
        
        
        // Equipment Effect
        int choice = EditorGUILayout.Popup("Add new Equipment Effect", -1, m_AvailableEquipEffectType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_AvailableEquipEffectType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_EquippedEffectListProperty.InsertArrayElementAtIndex(m_EquippedEffectListProperty.arraySize);
            m_EquippedEffectListProperty.GetArrayElementAtIndex(m_EquippedEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }


        Editor ed = null;
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
            m_EquippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        // Attack Effect
        choice = EditorGUILayout.Popup("Add new Weapon Attack Effect", -1, m_AvailableWeaponAttackEffectType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_AvailableWeaponAttackEffectType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_WeaponAttackEffectListProperty.InsertArrayElementAtIndex(m_WeaponAttackEffectListProperty.arraySize);
            m_WeaponAttackEffectListProperty.GetArrayElementAtIndex(m_WeaponAttackEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }

        toDelete = -1;
        for (int i = 0; i < m_WeaponAttackEffectListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_WeaponAttackEffectListProperty.GetArrayElementAtIndex(i);
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
            var item = m_WeaponAttackEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            //need to do it twice, first time just nullify the entry, second actually remove it.
            m_WeaponAttackEffectListProperty.DeleteArrayElementAtIndex(toDelete);
            m_WeaponAttackEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif