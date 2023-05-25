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

        void Start()
        {
            Character = GameObject.Find("Character").gameObject.GetComponent<UnitInfo>();
            CameraRay = Camera.main.gameObject.GetComponent<CameraRay>();
        }

        void Update()
        {
             
        }

        public void SetBtn()
        {
            Character.SaveUnitInfo();
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
            SceneManager.LoadScene("BattleScene");
        }

        public void SceneStart()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}