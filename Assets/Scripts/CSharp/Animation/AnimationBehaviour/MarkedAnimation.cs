using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedAnimation : StateMachineBehaviour
{
    Entity entity;
    [SerializeField] Type type;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entity == null)
        {
            entity = animator.GetComponentInParent<Entity>();
        }
        SetBool(true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetBool(false);
    }

    void SetBool(bool _bool)
    {
        switch (type)
        {
            case Type.Attack:
                if (_bool)
                    entity.OnAttackStarted();
                if (!_bool)
                    entity.OnAttackStopped();
                entity.isAttacking = _bool;
                break;
            case Type.Dash:
                entity.isDashing = _bool;
                break;
            case Type.Knockback:
                entity.isInKnockback = _bool;
                break;
            default:
                break;
        }
    }

    public enum Type
    {
        None,
        Attack,
        Dash,
        Knockback
    }
}
