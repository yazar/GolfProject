using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BallSettings", menuName = "Scriptable Objects/BallSettings")]
public class BallSettings : ScriptableObject
{
    public List<BallSetting> ballSettingsLibrary;
}
[Serializable]
public struct BallSetting
{
    public int ballLevel;
    public float points;
}