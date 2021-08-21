using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public static class CrippleBolt
{
    public static Vector2 quaternionVector2Multiply(Quaternion quaternion, Vector2 vector2) => quaternion * vector2;
    public static Vector3 quaternionVector3Multiply(Quaternion quaternion, Vector3 vector3) => quaternion * vector3;
}
