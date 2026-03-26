using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PathVisualizationSettings", menuName = "Scriptable Objects/Path Visualization Settings")]
public class PathVisualizationSettings : ScriptableObject
{
    public float lineWidth = 0.15f;
    public float lineHeightOffset = 0.2f;
    public Color[] tripColors = new Color[]
    {
        new Color(0.2f, 0.8f, 0.2f),
        new Color(0.2f, 0.6f, 1.0f),
        new Color(1.0f, 0.6f, 0.2f),
        new Color(0.9f, 0.2f, 0.9f),
        new Color(1.0f, 1.0f, 0.2f),
    };
    public float ballInfoHeightOffset = 0.5f;
    public Vector3 ballInfoScale = Vector3.one;
}
