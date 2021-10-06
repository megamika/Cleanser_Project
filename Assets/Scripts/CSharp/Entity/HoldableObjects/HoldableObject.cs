using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Addons.Community;

public class HoldableObject : MonoBehaviour
{
    public Transform rigTransform;
    public Transform parent;

    public HoldableSlot equipped;

    public HoldableSlot unEquipped;

    public void LateUpdate()
    {

    }

    public enum Type
    {
        None,
        Damaging,
    }

    public void SetupAllTransforms()
    {
        equipped.runtimeParent = PathRecorder.GetTransformFromPath(transform.parent, equipped.path);
        unEquipped.runtimeParent = PathRecorder.GetTransformFromPath(transform.parent, unEquipped.path);
    }

    public void SetupSlotInfo(ref HoldableSlot slot)
    {
        Transform parentBefore = transform.parent;
        transform.parent = parent;

        slot = new HoldableSlot
        {
            path = PathRecorder.RecordPath(rigTransform, parent),
            positionOffset = transform.localPosition,
            rotationOffset = transform.localEulerAngles
        };

        transform.parent = parentBefore;
    }

    public void UpdateParent(HoldableSlot slot)
    {
        transform.position = slot.runtimeParent.transform.position + slot.runtimeParent.rotation * Vector3.Scale(slot.positionOffset, slot.runtimeParent.lossyScale);
        transform.rotation = slot.runtimeParent.transform.rotation * Quaternion.Euler(slot.rotationOffset);
    }
}

[System.Serializable]
public struct HoldableSlot
{
    public string[] path;
    public Vector3 rotationOffset;
    public Vector3 positionOffset;

    public Transform runtimeParent;
}