using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    public List<AudioData> fxLibrary;
    public List<AudioData> musicLibrary;
}


public enum SoundType
{
    None,
    Music,
    Tap,
    Button,
    Step,
}