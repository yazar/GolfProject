using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioData", menuName = "Scriptable Objects/Audio Data")]
public class AudioData : ScriptableObject
{
    public SoundType soundType;
    public AudioClip audioClip;
    [Range(0f, 1f)]public float volume = 0.5f;
    
    [Range(-5f, 3f)]
    public float pitch = 1f;
    public bool randomizePitch;
    public float randomizationAmount = .04f;

    public void PlaySound(AudioSource testSource)
    {
        testSource.clip = audioClip;
        testSource.volume = volume;
        
        testSource.Play();
    }



}
