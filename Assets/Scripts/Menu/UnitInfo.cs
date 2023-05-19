using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

namespace ProjectCode
{
    public class UnitInfo : MonoBehaviour
    {
        private List<Item> HeadList;
        private List<Item> TorsoList;
        private List<Item> ArmList;
        private List<Item> LegList;
        private List<Item> WeaponList;

        private void Awake()
        {
            Debug.Log("Found item list");
            var item = Resources.LoadAll<GameObject>("Prefabs/Parts");
            for (int i = 0; i < item.Length; i++)
            {
                string name = item[i].GetComponent<PartInfo>().part.ItemName;
                var part = item[i].GetComponent<PartInfo>().part as MechEquipment;
                Debug.Log(name+"("+ item[i].GetComponent<PartInfo>().part.name + ")");
            }
            Debug.Log("==================");
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private List<CharacterData> Unit;
        public void SaveUnitInfo(CharacterData unit)
        {
            Unit.Add(unit);
            Debug.Log("Save Unit Info");
        }

        public void InitUnitInfo()
        {
            Debug.Log("Init Unit Info");
        }

        public void GetNextPart(MechEquipment.MechSlot partName)
        {
            switch(partName)
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