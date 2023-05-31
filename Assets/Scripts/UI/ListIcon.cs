using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

namespace ProjectCode
{
    public class ListIcon : MonoBehaviour
    {
        public int no;
        public void Clicked()
        {
            gameObject.SendMessage("GetUnitFromList", no);
        }

    }
}