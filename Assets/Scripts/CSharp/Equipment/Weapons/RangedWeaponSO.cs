using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Weapons/Ranged")]
public class RangedWeaponSO : ScriptableObject
{
    public GameObject weaponObject;
    public DamageStat damageStat;
    public RechargingCooldownStat rechargingCooldown;
}
