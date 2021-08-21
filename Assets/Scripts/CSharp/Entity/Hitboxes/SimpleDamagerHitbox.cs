using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDamagerHitbox : MonoBehaviour, Hitbox
{
    List<Entity> entitiesInside = new List<Entity>();
    public Entity parent { get; private set; }
    public HitboxType hitboxType => HitboxType.Damager;

    void Awake()
    {
        parent = GetComponentInParent<Entity>();
    }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Hitboxes");
    }

    public void DamageEntitiesInside(float damage)
    {
        foreach (var entity in entitiesInside)
        {
            entity.RecieveDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hitbox = other.gameObject.GetComponent<Hitbox>();
        if (!checkHitbox(hitbox)) return;
        entitiesInside.Add(hitbox.parent);
    }

    private void OnTriggerExit(Collider other)
    {
        Hitbox hitbox = other.gameObject.GetComponent<Hitbox>();
        if (!checkHitbox(hitbox)) return;
        entitiesInside.Remove(hitbox.parent);
    }

    bool checkHitbox(Hitbox hitbox)
    {
        if (hitbox.hitboxType != HitboxType.Damaged) return false;
        var relationshipWithHitbox = parent._stats.team.findRelationshipWith(hitbox.parent._stats.team);
        if (relationshipWithHitbox == null) return false;
        if (!relationshipWithHitbox.canDamage) return false;
        return true;
    }
}
