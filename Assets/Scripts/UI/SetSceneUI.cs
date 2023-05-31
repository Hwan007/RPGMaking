using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectCode
{
    public class SetSceneUI : MonoBehaviour
    {
        CameraRay CameraRay;
        UnitInfo Character;
        Text info;

        public GameObject NotEnoughParts;
        void Start()
        {
            Character = GameObject.Find("Character").gameObject.GetComponent<UnitInfo>();
            CameraRay = Camera.main.gameObject.GetComponent<CameraRay>();
        }

        void Update()
        {
            if (popup == true)
            {
                if (transform.Find("popup").childCount == 0)
                    popup = false;
            }
        }

        bool popup = false;
        public void SetBtn()
        {
            bool ret = false;
            ret = Character.UpdateUnitInfo();

            if (ret == false && popup == false)
            {
                GameObject Prefab = Instantiate(NotEnoughParts);
                Prefab.transform.SetParent(transform.Find("popup"));
                Prefab.transform.localPosition = Vector3.zero;
                Prefab.transform.localScale = Vector3.one;
                popup = true;
            }
            else
            {
                AddUnitToList(Character.Unit.Count-1);
                Character.InitUnit();
            }
        }
        public GameObject ListIcon;
        class ListInfo
        {
            public int no;
            public GameObject icon;
        }
        List<ListInfo> Lists = new List<ListInfo>();
        public void AddUnitToList(int i)
        {
            var unit = Instantiate(ListIcon, transform.GetChild(transform.childCount-1).GetChild(0).GetChild(0).GetChild(0));
            unit.name = "List " + (Character.Unit.Count-1);
            var script = unit.GetComponent<ListIcon>();
            script.no = Character.Unit[Character.Unit.Count - 1].index;
            ListInfo info = new ListInfo { no = script.no, icon = unit };
            Lists.Add(info);
        }

        public void DeleteUnitBtn()
        {
            
        }

        public void ClearUnitBtn()
        {
            Character.InitUnit();
        }

        public void ClearListBtn()
        {
            Character.ClearUnitList();
        }

        public void RightArrowBtn()
        {
            var click = CameraRay.SelectedPart.name;
            MechEquipment.MechSlot select = (MechEquipment.MechSlot) 99;
            switch(click)
            {
                case "head":
                    select = MechEquipment.MechSlot.head;
                    break;
                case "torso":
                    select = MechEquipment.MechSlot.torso;
                    break;
                case "right arm":
                case "left arm":
                    select = MechEquipment.MechSlot.arm;
                    break;
                case "right leg":
                case "left leg":
                    select = MechEquipment.MechSlot.leg;
                    break;
                case "right weapon":
                    select = MechEquipment.MechSlot.right_weapon;
                    break;
                case "left weapon":
                    select = MechEquipment.MechSlot.left_weapon;
                    break;
                default:
                    break;
            }
            Character.GetNextPart(select);
        }

        public void LeftArrowBtn()
        {
            var click = CameraRay.SelectedPart.name;
            MechEquipment.MechSlot select = (MechEquipment.MechSlot)99;
            switch (click)
            {
                case "head":
                    select = MechEquipment.MechSlot.head;
                    break;
                case "torso":
                    select = MechEquipment.MechSlot.torso;
                    break;
                case "right arm":
                case "left arm":
                    select = MechEquipment.MechSlot.arm;
                    break;
                case "right leg":
                case "left leg":
                    select = MechEquipment.MechSlot.leg;
                    break;
                case "right weapon":
                    select = MechEquipment.MechSlot.right_weapon;
                    break;
                case "left weapon":
                    select = MechEquipment.MechSlot.left_weapon;
                    break;
                default:
                    break;
            }
            Character.GetPreviousPart(select);
        }

        public void SceneBattle()
        {
            SaveUnitList.UnitList.Ally.Clear();
            Character.SaveUnitInfo();
            SceneManager.LoadScene("BattleScene");
        }

        public void SceneMainMenu()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}