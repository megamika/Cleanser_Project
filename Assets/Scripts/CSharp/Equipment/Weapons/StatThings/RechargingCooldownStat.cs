using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct RechargingCooldownStat
{
    public bool isAutomatic;

    public float singleShotCooldown;
    public float singleShotDrain;

    public float barCapacity;

    public float rechargeSpeed;
    public float timeBeforeRechargeStart;

    public float minFillAmountBeforeAvailible;
}
