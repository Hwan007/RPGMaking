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

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectCode.Material))]
public class MaterialEditor : Editor
{
    ProjectCode.Material m_Target;

    ItemEditor m_ItemEditor;

    [MenuItem("Assets/Create/Item/Material", priority = -999)]
    static public void CreateMaterial()
    {
        var newMaterial = CreateInstance<ProjectCode.Material>();

        ProjectWindowUtil.CreateAsset(newMaterial, "material.asset");
    }
}
#endif