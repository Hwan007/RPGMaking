using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectCode;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectCode
{
    public class HighlightableObject : MonoBehaviour
    {
        protected Renderer[] m_Renderers;

        int m_RimColorID;
        int m_RimPowID;
        int m_RimIntensityID;


        Color[] m_OriginalRimColor;
        float[] m_SavedRimIntensity;

        MaterialPropertyBlock m_PropertyBlock;
        protected virtual void Start()
        {
            m_Renderers = GetComponentsInChildren<Renderer>();

            m_RimColorID = Shader.PropertyToID("_RimColor");
            m_RimPowID = Shader.PropertyToID("_RimPower");
            m_RimIntensityID = Shader.PropertyToID("_RimIntensity");

            m_PropertyBlock = new MaterialPropertyBlock();

            m_OriginalRimColor = new Color[m_Renderers.Length];
            m_SavedRimIntensity = new float[m_Renderers.Length];
        }

        public void Highlight()
        {

        }

        public void Dehighlight()
        {

        }
    }
}