using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHitbox : MonoBehaviour, Hitbox
{
    public Entity parent { get; private set; }

    public HitboxType hitboxType => HitboxType.Damaged;

    void Awake()
    {
        parent = GetComponentInParent<Entity>();
    }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Hitboxes");
    }

}
