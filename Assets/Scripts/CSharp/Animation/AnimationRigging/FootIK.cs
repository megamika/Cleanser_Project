using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    Animator anim;
    LayerMask groundLayer;
    public float distanceToGround;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void OnAnimatorIK(int layerIndex)
    {
        DoTheFootIkThing(AvatarIKGoal.LeftFoot);
        DoTheFootIkThing(AvatarIKGoal.RightFoot);
    }

    void DoTheFootIkThing(AvatarIKGoal ikGoal)
    {
        anim.SetIKPositionWeight(ikGoal, 1f);
        anim.SetIKRotationWeight(ikGoal, 1f);

        RaycastHit hit;
        Ray ray = new Ray(anim.GetIKPosition(ikGoal) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distanceToGround + 1f, groundLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            anim.SetIKPosition(ikGoal, footPosition);
            anim.SetIKRotation(ikGoal, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }
}
