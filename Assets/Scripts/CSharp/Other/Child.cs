using UnityEngine;
using System.Collections;

public class Child : MonoBehaviour
{

    public Transform parentTransform;

    // If true, will attempt to scale the child accurately as the parent scales
    // Will not be accurate if starting rotations are different or irregular
    // Experimental
    public bool attemptChildScale = false;

    Vector3 startParentPosition;
    Quaternion startParentRotationQ;
    Vector3 startParentScale;

    Vector3 startChildPosition;
    Quaternion startChildRotationQ;

    Matrix4x4 parentMatrix;

    void Start()
    {

        startParentPosition = parentTransform.position;
        startParentRotationQ = parentTransform.rotation;

        startChildPosition = transform.position;
        startChildRotationQ = transform.rotation;

        // Keeps child position from being modified at the start by the parent's initial transform
        startChildPosition = DivideVectors(Quaternion.Inverse(parentTransform.rotation) * (startChildPosition - startParentPosition), startParentScale);
    }

    void Update()
    {

        parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);

        transform.position = parentMatrix.MultiplyPoint3x4(startChildPosition);

        transform.rotation = (parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
    }

    Vector3 DivideVectors(Vector3 num, Vector3 den)
    {

        return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);

    }
}