using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Addons.Community;

public class HoldableObject : MonoBehaviour
{
    Entity entity;

    public Type type;

    public Transform rigTransform;
    public string[] path;

    [Header("Stats")]
    [SerializeField] float damage;

    [Header("ChildOf")]
    public Transform parent;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] Vector3 positionOffset;

    public List<Entity> entitiesInside = new List<Entity>();

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entity.OnSwingStart += ()=> DefinedEvent.Trigger(gameObject, new DefinedEvents.Visuals.StartSwinging());
        entity.OnSwingEnd += ()=> DefinedEvent.Trigger(gameObject, new DefinedEvents.Visuals.StopSwinging());
    }

    private void Start()
    {
        switch (type)
        {
            case Type.None:
                break;
            case Type.Damaging:
                gameObject.layer = LayerMask.NameToLayer("Hitboxes");
                break;
            default:
                break;
        }
    }

    public void LateUpdate()
    {
       transform.position = parent.transform.position + parent.transform.rotation * positionOffset;
       transform.rotation = parent.transform.rotation * Quaternion.Euler(rotationOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<HitboxTransform>().entity;
        if (otherEntity == entity)
            return;
        entitiesInside.Add(otherEntity);
    }

    private void OnTriggerExit(Collider other)
    {
        Entity otherEntity = other.GetComponent<HitboxTransform>().entity;
        if (otherEntity == entity)
            return;
        entitiesInside.Remove(otherEntity);
    }

    public void Damage(float damageMultiplyer)
    {
        foreach (var damagedEntity in entitiesInside)
        {
            damagedEntity.Damage(new DamageData { damage = damage * damageMultiplyer, damager = damagedEntity, knockback = 0f, source = entity.transform.position });
        }
    }

    public enum Type
    {
        None,
        Damaging,
    }
}
