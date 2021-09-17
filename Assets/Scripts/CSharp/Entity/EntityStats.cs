using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Stats", menuName = "Stats/Entity")]
public class EntityStats : ScriptableObject
{
    public Team team;
    [Header("Health")]
    public float MaxHealth = 10f;

    [Header("Movement")]
    [Rename("Speed Multiplyer")]
    public float movementSpeed;
    [Rename("When Attacking Speed")]
    public float movementSpeedWhenAttacking;

    [Header("Gravity")]
    public float GravityForce = 9.8f;

    [Header("Rotation")]
    [Rename("Damping")]
    public float rotationDamping;
    public float attackDamping;

    [Header("Attack")]
    public float comboClearTime;

}

