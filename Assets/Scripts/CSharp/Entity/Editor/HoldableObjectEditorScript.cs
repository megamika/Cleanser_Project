using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HoldableObject))]
public class HoldableObjectEditorScript : Editor
{
    HoldableObject holdableObject;

    public override void OnInspectorGUI()
    {
        holdableObject = target as HoldableObject;

        if (GUILayout.Button("Record equipped parent"))
        {
            RecordEquippedPath();
        }

        if (GUILayout.Button("Record unequipped parent"))
        {
            RecordUnEquippedPath();
        }

        GUILayout.Space(30);

        if (GUILayout.Button("Snap to equipped"))
        {
            holdableObject.SetupAllTransforms();
            holdableObject.UpdateParent(holdableObject.equipped);
        }

        if (GUILayout.Button("Snap to unequipped"))
        {
            holdableObject.SetupAllTransforms();
            holdableObject.UpdateParent(holdableObject.unEquipped);
        }

        base.OnInspectorGUI();
    }

    void RecordEquippedPath()
    {
        holdableObject.SetupSlotInfo(ref holdableObject.equipped);
    }

    void RecordUnEquippedPath()
    {
        holdableObject.SetupSlotInfo(ref holdableObject.unEquipped);
    }
}
