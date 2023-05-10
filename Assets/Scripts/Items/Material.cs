using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
	public class Material : Item
	{
		public string Info;
		public override string GetDescription()
        {
			var desc = base.GetDescription();
			desc += "\n" + Info;
			return desc;
        }
		public string Name;
		public int Level;
		public float Density;
		public float DamageCorrection;
		public float AccuracyCorrection;
	}
}