using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitboxApply))]
public class HitboxApplyEditorScript : Editor
{
    HitboxApply hitboxApply;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Apply"))
        {
            AssaignValues();
            hitboxApply.Apply();
        }
        if (GUILayout.Button("Clear"))
        {
            AssaignValues();
            hitboxApply.Clear();
        }
        base.OnInspectorGUI();
    }

    void AssaignValues()
    {
        hitboxApply = (HitboxApply)target;
    }
}
