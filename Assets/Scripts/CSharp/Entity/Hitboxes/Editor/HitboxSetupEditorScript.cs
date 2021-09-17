using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitboxSetup))]
public class HitboxSetupEditorScript : Editor
{
    HitboxSetup hitboxSetup;


    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Symmetrize"))
        {
            Symmetrize();
        }
        if (GUILayout.Button("Record"))
        {
            AssignValues();
            hitboxSetup.RecordHitboxInfo();
        }
        base.OnInspectorGUI();
    }
    void AssignValues()
    {
        hitboxSetup = (HitboxSetup)target;
    }

    void Symmetrize()
    {
        AssignValues();

        Transform mainTransform = hitboxSetup.transform;
        Transform[] allTransforms = mainTransform.GetComponentsInChildren<Transform>();
        Dictionary<string, Transform> allTransformsIndexed = new Dictionary<string, Transform>();
        
        foreach (var transform in allTransforms)
            allTransformsIndexed.Add(transform.name, transform);
        
        foreach (var transform in allTransforms)
        {
            if (transform == mainTransform)
            {
                continue;
            }

            BoxCollider boxCollider = transform.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                continue;
            }

            MirrorTransformBox(".L", ".R");
            MirrorTransformBox(".R", ".L");

            void MirrorTransformBox(string side, string otherSide)
            {
                if (transform.gameObject.name.EndsWith(side))
                {
                    char[] charactersToTrim = { side[0], side[1] };
                    string nameWithoutMirror = transform.gameObject.name.TrimEnd(charactersToTrim);
                    if (allTransformsIndexed.ContainsKey(nameWithoutMirror + otherSide))
                    {
                        Transform mirroredTransform = allTransformsIndexed[nameWithoutMirror + otherSide];
                        BoxCollider mirroredBoxCollider = mirroredTransform.gameObject.GetComponent<BoxCollider>();
                        if (mirroredBoxCollider == null)
                        {
                            mirroredBoxCollider = mirroredTransform.gameObject.AddComponent<BoxCollider>();
                        }
                        mirroredBoxCollider.center = boxCollider.center;
                        mirroredBoxCollider.size = boxCollider.size;
                    }
                }
            }

        }


    }

}
