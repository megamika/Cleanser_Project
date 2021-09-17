using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PathRecorder
{
    public static string[] RecordPath(Transform mainTransform, Transform transform)
    {
        List<string> path = new List<string>();
        Transform currentTransform = transform.transform;
        while (true)
        {
            if (currentTransform == mainTransform)
            {
                break;
            }
            path.Add(currentTransform.name);
            currentTransform = currentTransform.parent;
        }

        string[] pathArray = path.ToArray();
        Array.Reverse(pathArray);
        return pathArray;
    }
}
