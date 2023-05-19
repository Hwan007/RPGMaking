using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

namespace ProjectCode
{
    public class UnitInfo : MonoBehaviour
    {
        private List<MechEquipment> HeadList = new List<MechEquipment>();
        private List<MechEquipment> TorsoList = new List<MechEquipment>();
        private List<MechEquipment> ArmList = new List<MechEquipment>();
        private List<MechEquipment> LegList = new List<MechEquipment>();
        private List<Weapon> WeaponList = new List<Weapon>();

        private void Awake()
        {
            LoadItem();
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
                    case (MechEquipment.MechSlot.Head):
                        HeadList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.Torso):
                        TorsoList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.Arm):
                        ArmList.Add(part[i]);
                        break;
                    case (MechEquipment.MechSlot.Leg):
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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private List<CharacterData> Unit = new List<CharacterData>();
        public void SaveUnitInfo(CharacterData unit)
        {
            Unit.Add(unit);
            Debug.Log("Save Unit Info");
        }

        public void ClearUnitInfo()
        {
            Unit.Clear();
            Debug.Log("Init Unit Info");
        }

        private int h=0, t=0, a=0, l=0, lw=0, rw=0;
        public void GetNextPart(MechEquipment.MechSlot partName)
        {
            Transform PrefabTransform;
            switch(partName)
            {
                case MechEquipment.MechSlot.Head:
                    Destroy(transform.Find("head").Find("part").GetChild(0).gameObject);
                    h++;
                    if (HeadList.Count < h)
                        h = 0;
                    PrefabTransform = Instantiate(HeadList[h].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("head").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.Torso:
                    Destroy(transform.Find("torso").Find("part").GetChild(0).gameObject);
                    t++;
                    if (TorsoList.Count < t)
                        t = 0;
                    PrefabTransform = Instantiate(TorsoList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("torso").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.Arm:
                    Destroy(transform.Find("arm").Find("part").GetChild(0).gameObject);
                    a++;
                    if (ArmList.Count < t)
                        a = 0;
                    PrefabTransform = Instantiate(ArmList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("arm").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.Leg:
                    Destroy(transform.Find("leg").Find("part").GetChild(0).gameObject);
                    l++;
                    if (LegList.Count < t)
                        l = 0;
                    PrefabTransform = Instantiate(LegList[t].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("leg").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.LeftWeapon:
                    Destroy(transform.Find("left weapon").Find("part").GetChild(0).gameObject);
                    lw++;
                    if (WeaponList.Count < t)
                        lw = 0;
                    PrefabTransform = Instantiate(WeaponList[lw].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("left weapon").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case MechEquipment.MechSlot.RightWeapon:
                    Destroy(transform.Find("right weapon").Find("part").GetChild(0).gameObject);
                    rw++;
                    if (WeaponList.Count < t)
                        rw = 0;
                    PrefabTransform = Instantiate(WeaponList[rw].WorldObjectPrefab).transform;
                    PrefabTransform.parent = transform.Find("right weapon").Find("part");
                    PrefabTransform.localPosition = new Vector3(0, 0, 0);
                    break;
            }
        }

        public void GetPreviousPart(MechEquipment.MechSlot partName)
        {
            switch (partName)
            {
                case MechEquipment.MechSlot.Head:
                    break;
                case MechEquipment.MechSlot.Torso:
                    break;
                case MechEquipment.MechSlot.Arm:
                    break;
                case MechEquipment.MechSlot.Leg:
                    break;
                case MechEquipment.MechSlot.LeftWeapon:
                    break;
                case MechEquipment.MechSlot.RightWeapon:
                    break;
            }
        }
    }
}