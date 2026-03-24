using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCalculator
{
    public static List<Vector3> CalculatedPath(Transform startPoint, List<Transform> targetPositions)
    {
        List<Vector3> path = new List<Vector3>();

        foreach (Transform target in targetPositions)
        {
            path.Add(target.position);
        }
        
        path.Add(startPoint.position);

        return path;
    }
}
