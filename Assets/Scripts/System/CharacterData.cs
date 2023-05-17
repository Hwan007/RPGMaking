using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public enum UnitAction
    {
        Move,
        StandAttack,
        ChargeAttack,
        Skill
    }
    public class BattleAction
    {
        public UnitAction Act;
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
            
            if (m_ActionCoolDown > 0.0f)
                m_ActionCoolDown -= Time.deltaTime;
            else
            {
                ActionQueue();
            }
            
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

        /// <summary>
        /// 행동을 마지막 순서에 넣는다.
        /// </summary>
        public void AddAction(BattleAction act)
        {
            Debug.Log("AddAction : " + act.Act + " / " + ActionList.Count);
            ActionList.Add(act);
        }

        /// <summary>
        /// 현재 수행중인 행동을 취소한다.
        /// 수행중이지 않는 경우에는 다음 행동을 취소한다.
        /// </summary>
        public void CancelAction()
        {
            BattleAction act = ActionList[0];
            Debug.Log("CancelAction : " + act.Act + " / " + ActionList.Count);
            if (act.OnActing == true)
            {
                m_ActionCoolDown += act.PostDelay;
                ActionList[0].count = 0;
                Debug.Log("ActionCoolDown : " + m_ActionCoolDown);
            }
            ActionList.RemoveAt(0);
            Debug.Log(" => " + " / " + ActionList.Count);
        }

        /// <summary>
        /// 마지막에 넣은 행동을 취소한다.
        /// 수행중인 경우에는 딜레이를 적용하고 취소한다.
        /// </summary>
        public void UndoAction()
        {
            if (ActionList[ActionList.Count-1].OnActing)
            {
                BattleAction act = ActionList[ActionList.Count-1];
                m_ActionCoolDown += act.PostDelay;
                ActionList[ActionList.Count - 1].count = 0;
            }
            ActionList.RemoveAt(ActionList.Count-1);
        }

        /// <summary>
        /// 행동 순서에 대한 딜레이를 넣는다.
        /// 행동 순서에 대한 동작을 불러온다.
        /// </summary>
        public void ActionQueue()
        {
            if (ActionList.Count != 0 && ActionList[0].OnActing == false)
            {
                BattleAction act = ActionList[0];
                ActionList[0].OnActing = true;
                Debug.Log("ActionList : " + act.Act + " / " + act.count);
                m_ActionCoolDown = act.PreDelay;// + act.PostDelay + act.Duration * act.count;
            }
            else if (ActionList[0].OnActing == true)
            {
                BattleAction act = ActionList[0];
                Debug.Log("ActionList : " + ActionList[0].Act + " / " + ActionList[0].count);
                if (act.count != 0)
                {
                    m_ActionCoolDown = act.Duration;
                    act.count--;
                    // 행동에 대한 동작을 수행한다.
                    Debug.Log("Act " + act.Act);
                    switch (act.Act)
                    {
                        case UnitAction.Move:
                            break;
                        case UnitAction.StandAttack:
                            break;
                        case UnitAction.ChargeAttack:
                            break;
                        case UnitAction.Skill:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    m_ActionCoolDown = act.PostDelay;
                    ActionList.RemoveAt(0);
                }
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