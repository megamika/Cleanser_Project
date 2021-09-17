using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New hitbox info", menuName = "Hitbox Info")]
public class HitboxInfoSO : ScriptableObject
{
    public HitboxInfo hitboxInfo;
}

[System.Serializable]
public struct HitboxInfo
{
    public Hitbox[] hitboxes;
}

[System.Serializable]
public struct Hitbox
{
    public string name;
    public string[] path;
    public Vector3 offset;
    public Vector3 size;
}
