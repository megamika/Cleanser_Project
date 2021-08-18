using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTransforms : MonoBehaviour
{
    Animator anim;
    public Vector3 deltaPosition { get; private set; }
    public Vector3 deltaPositionUnscaled => deltaPosition / Time.deltaTime;
    public Quaternion deltaRotation { get; private set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        deltaPosition = anim.deltaPosition;
        deltaRotation = anim.deltaRotation;
    }
}
