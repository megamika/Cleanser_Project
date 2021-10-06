using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mele Weapon", menuName = "Weapons/Mele")]
public class MeleWeaponSO : ScriptableObject
{
    public GameObject weaponObject;
    public DamageStat damageStat;
}
