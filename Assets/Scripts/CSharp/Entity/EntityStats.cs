using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Stats", menuName = "Stats/Entity")]
public class EntityStats : ScriptableObject
{
    [Header("Movement")]
    [Rename("Speed Multiplyer")]
    public float movementSpeed;
    [Header("Rotation")]
    [Rename("Damping")]
    public float rotationDamping;
}
