using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;
using System;

namespace ProjectCode
{
    public class UnitInfo : MonoBehaviour
    {
        private List<MechEquipment> HeadList = new List<MechEquipment>();
        private List<MechEquipment> TorsoList = new List<MechEquipment>();
        private List<MechEquipment> ArmList = new List<MechEquipment>();
        private List<MechEquipment> LegList = new List<MechEquipment>();
        private List<Weapon> WeaponList = new List<Weapon>();

        private List<CharacterData> Unit = new List<CharacterData>();

        private int h = 0, t = 0, a = 0, l = 0, lw = 0, rw = 0;

        private string[] PartName =
        {
            "Parts/head",
            "Parts/torso",
            "Parts/arm",
            "Parts/leg",
            "Weapon/left_weapon",
            "Weapon/right_weapon"
        };

        private void Awake()
        {
            LoadItem();
        }

        public void Start()
        {
            InitItem();
        }

        public void InitItem()
        {
            Transform PrefabTransform;

            for (int no = 0; no < PartName.Length; no++)
            {
                Transform par = transform.Find(PartName[no]).Find("part");
                if (par.childCount > 0)
                    Destroy(par.GetChild(0).gameObject);

                if (no == 0 && HeadList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(HeadList[0].WorldObjectPrefab).transform;
                else if (no == 1 && TorsoList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(TorsoList[0].WorldObjectPrefab).transform;
                else if (no == 2 && ArmList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(ArmList[0].WorldObjectPrefab).transform;
                else if (no == 3 && LegList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(LegList[0].WorldObjectPrefab).transform;
                else if (no == 4 && WeaponList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(WeaponList[0].WorldObjectPrefab).transform;
                else if (no == 5 && WeaponList[0].WorldObjectPrefab != null)
                    PrefabTransform = Instantiate(WeaponList[0].WorldObjectPrefab).transform;
                else
                    PrefabTransform = null;

                if (PrefabTransform != null)
                {
                    PrefabTransform.parent = par;
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
        public void LoadItem()
        {
            Debug.Log("Found item list");

            // mech part item list
            var part = Resources.LoadAll<MechEquipment>("Prefabs/Parts");
            for (int i = 0; i < part.Length; i++)
            {
                string name = part[i].ItemName;

                MechEquipment.MechSlot partInfo = part[i].SlotStat.PartSlot;
                Debug.Log(name + "(" + partInfo + ")");
                switch (partInfo)
                {
                    case (MechEquipment.MechSlot.head):
                        HeadList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.torso):
                        TorsoList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.arm):
                        ArmList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.leg):
                        LegList.Add(part[i]);
                        break;
                    default:
                        Debug.Log("Wrong part info : " + partInfo);
                        break;
                }
            }

            // weapon item list
            var weapon = Resources.LoadAll<Weapon>("Prefabs/Weapon");
            for (int i = 0; i < weapon.Length; i++)
            {
                string name = weapon[i].ItemName;
                Debug.Log(name + "(Weapon)");
                WeaponList.Add(weapon[i]);
            }
            Debug.Log("==================");


            MechEquipment temp = ScriptableObject.CreateInstance<MechEquipment>();
            HeadList.Insert(0, temp);
            TorsoList.Insert(0, temp);
            ArmList.Insert(0, temp);
            LegList.Insert(0, temp);
            Weapon tempwea = ScriptableObject.CreateInstance<Weapon>();
            WeaponList.Insert(0, tempwea);
        }

        public void ClearItem()
        {
            HeadList.Clear();
            TorsoList.Clear();
            ArmList.Clear();
            LegList.Clear();
            WeaponList.Clear();
            Debug.Log("Clear item list");
        }

        public void SaveUnitInfo()
        {
            CharacterData unit = new CharacterData();
            unit.Equipment.Equip(MechEquipment.MechSlot.head, HeadList[h]);
            unit.Equipment.Equip(MechEquipment.MechSlot.torso, TorsoList[t]);
            unit.Equipment.Equip(MechEquipment.MechSlot.arm, ArmList[a]);
            unit.Equipment.Equip(MechEquipment.MechSlot.leg, LegList[l]);
            unit.Equipment.Equip(MechEquipment.MechSlot.right_weapon, WeaponList[rw]);
            unit.Equipment.Equip(MechEquipment.MechSlot.left_weapon, WeaponList[lw]);
            // pilot Ãß°¡
            Unit.Add(unit);
            Debug.Log("Save Unit Info");
        }

        public void ClearUnitInfo()
        {
            Unit.Clear();
            Debug.Log("Init Unit Info");
        }

        public void ArrowNext(string part)
        {
            var ret = (MechEquipment.MechSlot)Enum.Parse(typeof(MechEquipment.MechSlot), part);
            GetNextPart(ret);
        }

        public void ArrowPrevious(string part)
        {
            var ret = (MechEquipment.MechSlot)Enum.Parse(typeof(MechEquipment.MechSlot), part);
            GetPreviousPart(ret);
        }

        public void GetNextPart(MechEquipment.MechSlot partName)
        {
            Transform PrefabTransform, part;
            string position;
            
            if ((int)partName < 4)
                position = "Parts/" + partName.ToString();
            else
                position = "Weapon/" + partName.ToString();
            part = transform.Find(position).Find("part");

            if (part.childCount > 0)
                Destroy(part.GetChild(0).gameObject);

            switch (partName)
            {
                case MechEquipment.MechSlot.head:
                    h++;
                    if (HeadList.Count-1 < h)
                        h = 0;
                    else
                    {
                        PrefabTransform = Instantiate(HeadList[h].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                case MechEquipment.MechSlot.torso:
                    t++;
                    if (TorsoList.Count-1 < t)
                        t = 0;
                    else
                    {
                        PrefabTransform = Instantiate(TorsoList[t].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                case MechEquipment.MechSlot.arm:
                    a++;
                    if (ArmList.Count-1 < a)
                        a = 0;
                    else
                    {
                        PrefabTransform = Instantiate(ArmList[t].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                case MechEquipment.MechSlot.leg:
                    l++;
                    if (LegList.Count-1 < l)
                        l = 0;
                    else
                    {
                        PrefabTransform = Instantiate(LegList[t].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                case MechEquipment.MechSlot.left_weapon:
                    lw++;
                    if (WeaponList.Count-1 < lw)
                        lw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[lw].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                case MechEquipment.MechSlot.right_weapon:
                    rw++;
                    if (WeaponList.Count-1 < rw)
                        rw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[rw].WorldObjectPrefab).transform;
                        PrefabTransform.parent = part;
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    }
                    break;
                default:
                    Debug.Log("Worng part number");
                    return;
            }
        }

        public void GetPreviousPart(MechEquipment.MechSlot partName)
        {
            Transform PrefabTransform;
            switch (partName)
            {
                case MechEquipment.MechSlot.head:
                    Destroy(transform.Find("head").Find("part").GetChild(0).gameObject);
                    h--;
                    if (HeadList.Count < h || h < 0)
                        h = 0;
                    PrefabTransform = Instantiate(HeadList[h].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("head").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.torso:
                    Destroy(transform.Find("torso").Find("part").GetChild(0).gameObject);
                    t--;
                    if (TorsoList.Count < t || t < 0)
                        t = 0;
                    PrefabTransform = Instantiate(TorsoList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("torso").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.arm:
                    Destroy(transform.Find("arm").Find("part").GetChild(0).gameObject);
                    a--;
                    if (ArmList.Count < a || a < 0)
                        a = 0;
                    PrefabTransform = Instantiate(ArmList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("arm").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.leg:
                    Destroy(transform.Find("leg").Find("part").GetChild(0).gameObject);
                    l--;
                    if (LegList.Count < l || l < 0)
                        l = 0;
                    PrefabTransform = Instantiate(LegList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("leg").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.left_weapon:
                    Destroy(transform.Find("left weapon").Find("part").GetChild(0).gameObject);
                    lw--;
                    if (WeaponList.Count < lw || lw < 0)
                        lw = 0;
                    PrefabTransform = Instantiate(WeaponList[lw].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("left weapon").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.right_weapon:
                    Destroy(transform.Find("right weapon").Find("part").GetChild(0).gameObject);
                    rw--;
                    if (WeaponList.Count < rw || rw < 0)
                        rw = 0;
                    PrefabTransform = Instantiate(WeaponList[rw].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("right weapon").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                default:
                    Debug.Log("Worng part number");
                    break;
            }
        }
    }
}