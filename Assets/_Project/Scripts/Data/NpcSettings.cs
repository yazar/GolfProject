using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NpcSettings", menuName = "Scriptable Objects/NpcSettings")]
public class NpcSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float speed;
    public float pickUpDuration;
    public float dropOffDuration;
    
    [Header("Health Settings")]
    public float healthDuration;

    [Header("General Settings")] 
    public int maxBallsToCarry;

}
