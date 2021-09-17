using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damager
{
    public GameObject gameObject { get; }
    public bool isEntity { get; }
    public Entity entity { get; }
}

public struct DamageData
{
    public Damager damager;
    public Vector3 source;
    public float damage;
    public float knockback;
}