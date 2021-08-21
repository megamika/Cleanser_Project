using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEventCatcher : MonoBehaviour
{
    Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void AttackDamage(int i)
    {
        Debug.Log("Trying to inflict damage");
        entity.DamageAnimationEvent(i);
    }

    public void AttackTransitionReady()
    {
        entity.readyToAttackTransition = true;
    }
}