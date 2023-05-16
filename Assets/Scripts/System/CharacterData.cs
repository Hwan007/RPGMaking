using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public enum ActionList
    {
        Move,
        StandAttack,
        ChargeAttack,
        Skill
    }
    public class BattleAction
    {
        public ActionList Act;
        public int count;
        public float PreDelay;
        public float Duration;
        public float PostDelay;
        public bool OnActing;
    }
    public class CharacterData : HighlightableObject
    {
        public string CharacterName;

        public StatSystem Stats;

        public EquipmentSystem Equipment = new EquipmentSystem();

        public AudioClip[] HitClip;

        public List<BattleAction> ActionList = new List<BattleAction>();

        public Action OnDamage { get; set; }

        public bool CanAction
        {
            get { return m_ActionCoolDown <= 0.0f; }
        }

        float m_ActionCoolDown;

        public void Init()
        {
            Stats.Init(this);
            Equipment.Init(this);
        }

        void Awake()
        {

        }

        void Update()
        {
            Stats.Tick();
            ActionQueue();
            
            if (m_ActionCoolDown > 0.0f)
                m_ActionCoolDown -= Time.deltaTime;
            
        }

        public bool CanAttackReach(CharacterData target, string selectedPart)
        {
            bool ret;
            if (selectedPart == "Right")
            {
                ret = Equipment.RightWeapon.CanHit(this, target);
            }
            else if (selectedPart == "Left")
            {
                ret = Equipment.LeftWeapon.CanHit(this, target);
            }
            else
                ret = false;
            return ret;
        }

        public bool CanAttackTarget(CharacterData target, string selectedPart)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;

            if (!CanAttackReach(target, selectedPart))
                return false;

            return true;
        }

        public void Death()
        {
            Stats.Death();
        }

        public void Attack(CharacterData target, MechEquipment.MechSlot part)
        {
            if (part == MechEquipment.MechSlot.LeftWeapon)
            {
                Equipment.LeftWeapon.Attack(this, target);
            }
            else if (part == MechEquipment.MechSlot.RightWeapon)
            {
                Equipment.RightWeapon.Attack(this, target);
            }
        }

        public void AddAction(BattleAction act)
        {
            ActionList.Add(act);
        }

        public void UndoAction()
        {
            if (ActionList[ActionList.Count].OnActing)
            {
                BattleAction act = ActionList[ActionList.Count];
                m_ActionCoolDown = act.PostDelay;
            }
            else
            {
                ActionList.RemoveAt(ActionList.Count);
            }
        }

        public void ActionQueue()
        {
            if (ActionList.Count != 0 && ActionList[0].OnActing == false)
            {
                BattleAction act = ActionList[0];
                Debug.Log("ActionList : " + act.Act + " / " + act.count);
                m_ActionCoolDown = act.PreDelay + act.PostDelay + act.Duration * act.count;
                ActionList[0].OnActing = true;
            }
            else if (ActionList[0].OnActing == false && m_ActionCoolDown <= 0)
            {
                Debug.Log("ActionList : " + ActionList[0].Act + " / " + ActionList[0].count);
                ActionList.RemoveAt(0);
            }
        }
        /*
        public void ActionTriggered(MechEquipment.MechSlot part, int count)
        {
            if (part == MechEquipment.MechSlot.LeftWeapon)
            {
                m_ActionCoolDown = Equipment.LeftWeapon.WeaponStats.PreDelay + Equipment.LeftWeapon.WeaponStats.Duration * count + Equipment.LeftWeapon.WeaponStats.PostDelay;
            }
            else if (part == MechEquipment.MechSlot.RightWeapon)
            {
                m_ActionCoolDown = Equipment.RightWeapon.WeaponStats.PreDelay + Equipment.RightWeapon.WeaponStats.Duration * count + Equipment.RightWeapon.WeaponStats.PostDelay;
            }
        }
        */
        public void Damage(Weapon.AttackData attackData)
        {
            /*
            if (HitClip.Length != 0)
            {
                SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
                {
                    Clip = HitClip[Random.Range(0, HitClip.Length)],
                    PitchMax = 1.1f,
                    PitchMin = 0.8f,
                    Position = transform.position
                });
            }
            */

            Stats.Damage(attackData);

            OnDamage?.Invoke();
        }
    }
}