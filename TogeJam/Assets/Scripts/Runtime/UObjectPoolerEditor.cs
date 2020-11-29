using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Utility
{
    [CustomEditor(typeof(UObjectPooler))]
    public class UObjectPoolerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("ReInit Pool"))
            {
                UObjectPooler Obj = target as UObjectPooler;
                if (Obj != null)
                    Obj.ReinitPool();
            }
        }
    }
}
