using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSetup : MonoBehaviour
{
    public HitboxInfo hitboxInfo;

    public void RecordHitboxInfo()
    {
        Transform mainTransform = transform;
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();
        Hitbox[] hitboxes = new Hitbox[boxColliders.Length];

        for (int i = 0; i < boxColliders.Length; i++)
        {
            string[] pathArray = PathRecorder.RecordPath(mainTransform, boxColliders[i].transform);
            hitboxes[i] = new Hitbox { offset = boxColliders[i].center, size = boxColliders[i].size, path = pathArray, name = boxColliders[i].name };
        }

        hitboxInfo = new HitboxInfo { hitboxes = hitboxes };
    }

}
