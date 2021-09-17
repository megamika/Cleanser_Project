using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxApply : MonoBehaviour
{
    public HitboxInfoSO hitboxInfo;

    public void Clear()
    {
        BoxCollider[] boxColliders = transform.GetComponentsInChildren<BoxCollider>();
        Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        HitboxTransform[] hitboxTransforms = transform.GetComponentsInChildren<HitboxTransform>();

        int boxCollLength = boxColliders.Length;
        for (int i = 0; i < boxCollLength; i++)
        {
            boxColliders[i].gameObject.layer = 0;
            DestroyImmediate(boxColliders[i]);
        }

        int rigidbodyLength = rigidbodies.Length;
        for (int i = 0; i < rigidbodyLength; i++)
        {
            DestroyImmediate(rigidbodies[i]);
        }

        int hitboxTransformsLength = hitboxTransforms.Length;
        for (int i = 0; i < hitboxTransformsLength; i++)
        {
            DestroyImmediate(hitboxTransforms[i]);
        }
    }

    public void Apply()
    {
        Clear();
        if (hitboxInfo == null) return;

        Hitbox[] hitboxes = hitboxInfo.hitboxInfo.hitboxes;
        bool failedToFindPath = false;
        for (int i = 0; i < hitboxes.Length; i++)
        {
            failedToFindPath = false;
            Transform hitboxTransform = transform;
            for (int j = 0; j < hitboxes[i].path.Length; j++)
            {
                hitboxTransform = hitboxTransform.Find(hitboxes[i].path[j]);
                if (hitboxTransform == null)
                {
                    failedToFindPath = true;
                    break;
                }
            }
            if (!failedToFindPath)
            {
                BoxCollider hitboxCollider = hitboxTransform.GetComponent<BoxCollider>();
                if (hitboxCollider == null)
                {
                    hitboxCollider = hitboxTransform.gameObject.AddComponent<BoxCollider>();
                }

                hitboxCollider.isTrigger = true;
                hitboxCollider.center = hitboxes[i].offset;
                hitboxCollider.size = hitboxes[i].size;
                hitboxCollider.gameObject.layer = LayerMask.NameToLayer("Hitboxes");

                Rigidbody rb = hitboxTransform.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = hitboxTransform.gameObject.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                HitboxTransform hitboxTransformComponent = hitboxCollider.gameObject.GetComponent<HitboxTransform>();
                if (hitboxTransformComponent == null)
                {
                    hitboxTransformComponent = hitboxCollider.gameObject.AddComponent<HitboxTransform>();
                }
                hitboxTransformComponent.entity = hitboxTransformComponent.GetComponentInParent<Entity>();
            }
        }
    }
}
