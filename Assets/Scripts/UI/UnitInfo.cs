using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCode
{
    public class UnitInfo : MonoBehaviour
    {
        private List<MechEquipment> HeadList = new List<MechEquipment>();
        private List<MechEquipment> TorsoList = new List<MechEquipment>();
        private List<MechEquipment> ArmList = new List<MechEquipment>();
        private List<MechEquipment> LegList = new List<MechEquipment>();
        private List<Weapon> WeaponList = new List<Weapon>();

        public List<EquipData> Unit = new List<EquipData>();

        public Text[] info = new Text[6];

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
            ClearUnitList();
        }

        public void Start()
        {
            LoadItem();
            InitUnit();
        }

        public void InitUnit()
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
                    PrefabTransform.SetParent(par);
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

        /// <summary>
        /// Clear Item List
        /// </summary>
        public void ClearItem()
        {
            HeadList.Clear();
            TorsoList.Clear();
            ArmList.Clear();
            LegList.Clear();
            WeaponList.Clear();
            Debug.Log("Clear item list");
        }
        int tempNo=0;
        /// <summary>
        /// Save Unit part information that user select
        /// </summary>
        /// <returns></returns>
        public bool UpdateUnitInfo()
        {
            bool ret = false;

            if (h == 0 || t == 0 || a == 0 || l == 0 || (rw == 0 && lw == 0))
                ret = false;
            else
            {
                EquipData unit = new EquipData();
                unit.index = tempNo++;
                unit.Equipment.Equip(MechEquipment.MechSlot.head, HeadList[h]);
                unit.Equipment.Equip(MechEquipment.MechSlot.torso, TorsoList[t]);
                unit.Equipment.Equip(MechEquipment.MechSlot.arm, ArmList[a]);
                unit.Equipment.Equip(MechEquipment.MechSlot.leg, LegList[l]);
                if (rw != 0)
                    unit.Equipment.Equip(MechEquipment.MechSlot.right_weapon, WeaponList[rw]);
                if (lw != 0)
                    unit.Equipment.Equip(MechEquipment.MechSlot.left_weapon, WeaponList[lw]);
                // pilot Ãß°¡
                Unit.Add(unit);
                Debug.Log("Save Unit Info");
                ret = true;
            }

            return ret;
        }

        public int ListIndex=0;
        public int GetUnitFromList(int ListInfoNo)
        {
            Transform PrefabTransform;
            ListIndex = ListInfoNo;
            for (int no = 0; no < PartName.Length; no++)
            {
                Transform par = transform.Find(PartName[no]).Find("part");
                if (par.childCount > 0)
                    Destroy(par.GetChild(0).gameObject);
                
                PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.head).WorldObjectPrefab, par).transform;
                PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.torso).WorldObjectPrefab, par).transform;
                PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.arm).WorldObjectPrefab, par).transform;
                PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.leg).WorldObjectPrefab, par).transform;
                if (Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.left_weapon) != null)
                    PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.left_weapon).WorldObjectPrefab, par).transform;
                if (Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.right_weapon) != null)
                    PrefabTransform = Instantiate(Unit[ListInfoNo].Equipment.GetItem(MechEquipment.MechSlot.right_weapon).WorldObjectPrefab, par).transform;
                
                if (PrefabTransform != null)
                {
                    PrefabTransform.localPosition = Vector3.zero;
                    PrefabTransform.localScale = Vector3.zero;
                }
            }
            return ListInfoNo;
        }

        public void SaveUnitInfo()
        {
            //SaveUnitList.UnitList.Ally.AddRange(Unit);
        }

        public void DeleteUnitInfo(int i)
        {
            Unit.RemoveAt(i);
            Debug.Log("Delete Unit info");
        }

        /// <summary>
        /// Clear Unit list information that user saved
        /// </summary>
        public void ClearUnitList()
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
                    if (HeadList.Count - 1 < h)
                        h = 0;
                    else
                    {
                        PrefabTransform = Instantiate(HeadList[h].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
                    }
                    break;

                case MechEquipment.MechSlot.torso:
                    t++;
                    if (TorsoList.Count - 1 < t)
                        t = 0;
                    else
                    {
                        PrefabTransform = Instantiate(TorsoList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
                    }
                    break;

                case MechEquipment.MechSlot.arm:
                    a++;
                    if (ArmList.Count - 1 < a)
                        a = 0;
                    else
                    {
                        PrefabTransform = Instantiate(ArmList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
                    }
                    break;

                case MechEquipment.MechSlot.leg:
                    l++;
                    if (LegList.Count - 1 < l)
                        l = 0;
                    else
                    {
                        PrefabTransform = Instantiate(LegList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
                    }
                    break;

                case MechEquipment.MechSlot.left_weapon:
                    lw++;
                    if (WeaponList.Count - 1 < lw)
                        lw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[lw].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
                    }
                    break;

                case MechEquipment.MechSlot.right_weapon:
                    rw++;
                    if (WeaponList.Count - 1 < rw)
                        rw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[rw].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(part);
                        PrefabTransform.localScale = Vector3.one;
                        PrefabTransform.localPosition = Vector3.zero;
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
                    {
                        h = 0;
                    }
                    else
                    {
                        PrefabTransform = Instantiate(HeadList[h].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("head").Find("part"));
                        PrefabTransform.localPosition = Vector3.zero;
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                case MechEquipment.MechSlot.torso:
                    Destroy(transform.Find("torso").Find("part").GetChild(0).gameObject);
                    t--;
                    if (TorsoList.Count < t || t < 0)
                    {
                        t = 0;
                    }
                    else
                    {
                        PrefabTransform = Instantiate(TorsoList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("torso").Find("part"));
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                case MechEquipment.MechSlot.arm:
                    Destroy(transform.Find("arm").Find("part").GetChild(0).gameObject);
                    a--;
                    if (ArmList.Count < a || a < 0)
                        a = 0;
                    else
                    {
                        PrefabTransform = Instantiate(ArmList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("arm").Find("part"));
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                case MechEquipment.MechSlot.leg:
                    Destroy(transform.Find("leg").Find("part").GetChild(0).gameObject);
                    l--;
                    if (LegList.Count < l || l < 0)
                    {
                        l = 0;
                    }
                    else
                    {
                        PrefabTransform = Instantiate(LegList[t].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("leg").Find("part"));
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                case MechEquipment.MechSlot.left_weapon:
                    Destroy(transform.Find("left weapon").Find("part").GetChild(0).gameObject);
                    lw--;
                    if (WeaponList.Count < lw || lw < 0)
                        lw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[lw].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("left weapon").Find("part"));
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                case MechEquipment.MechSlot.right_weapon:
                    Destroy(transform.Find("right weapon").Find("part").GetChild(0).gameObject);
                    rw--;
                    if (WeaponList.Count < rw || rw < 0)
                        rw = 0;
                    else
                    {
                        PrefabTransform = Instantiate(WeaponList[rw].WorldObjectPrefab).transform;
                        PrefabTransform.SetParent(transform.Find("right weapon").Find("part"));
                        PrefabTransform.localPosition = new Vector3(0, 0, 0);
                        PrefabTransform.localScale = Vector3.one;
                    }
                    break;

                default:
                    Debug.Log("Worng part number");
                    break;
            }
        }
    }
}