using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Throwable Weapon", menuName = "Weapons/Throwable")]
public class ThrowableWeaponsSO : ScriptableObject
{
    public GameObject weaponObject;
    public DamageStat damageStat;
    public int ammoPerShot;
}
