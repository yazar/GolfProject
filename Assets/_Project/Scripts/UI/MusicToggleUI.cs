using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggleUI : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject musicOnImage;
    [SerializeField] GameObject musicOffImage;

    private void Start()
    {
        bool isOn = audioManager.musicOn;
        musicOnImage.SetActive(isOn);
        musicOffImage.SetActive(!isOn);
    }

    public void ToggleMusic()
    {
        audioManager.SwitchMusicState();
        audioManager.SwitchSoundState();
        
        bool isOn = audioManager.musicOn;
        musicOnImage.SetActive(isOn);
        musicOffImage.SetActive(!isOn);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnSoundToggle += ToggleMusic;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSoundToggle -= ToggleMusic;
    }
}
