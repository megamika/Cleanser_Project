using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedObjects : MonoBehaviour
{
    Entity entity;

    [SerializeField] GameObject[] weapons;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(entity.isAttacking);
        }
    }

    public void DealDamage(float multiplyer)
    {
        foreach (var weapon in weapons)
        {
            weapon.GetComponent<HoldableObject>().Damage(1f);
        }
    }
}
