using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HoldableObject))]
public class HoldableObjectEditorScript : Editor
{
    HoldableObject holdableObject;
    bool previewEnabled;

    public override void OnInspectorGUI()
    {
        holdableObject = target as HoldableObject;

        if (!previewEnabled)
        {
            if (GUILayout.Button("Start child preview"))
            {
                previewEnabled = true;
            }
        }
        else
        {
            if (GUILayout.Button("Stop child preview"))
            {
                previewEnabled = false;
                holdableObject.transform.localPosition = Vector3.zero;
                holdableObject.transform.localRotation = Quaternion.identity;
            }
        }

        if (GUILayout.Button("Record parent"))
        {
            RecordPath();
        }

        base.OnInspectorGUI();
    }

    void RecordPath()
    {
        holdableObject.path = PathRecorder.RecordPath(holdableObject.rigTransform, holdableObject.parent);
    }

    private void OnSceneGUI()
    {
        if (previewEnabled)
        {
            holdableObject.LateUpdate();
        }
    }
}
