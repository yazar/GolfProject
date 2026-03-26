using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NpcSettings", menuName = "Scriptable Objects/NpcSettings")]
public class NpcSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float acceleration = 8f;
    public float pickUpDuration = 2f;
    public float dropOffDuration = 2f;
    public float stoppingDistance = 0.5f;
    
    [Header("Health Settings")]
    public float healthDuration = 30f;

    [Header("General Settings")] 
    public int maxBallsToCarry = 5;
    
    [Header("Path Visualization Settings")] 
    public bool showVisualizationOnStart = true;
    public bool updateLineRenderers = true;
    public PathVisualizationSettings settingsOnTps;
    public PathVisualizationSettings settingsOnTopdown;

}
