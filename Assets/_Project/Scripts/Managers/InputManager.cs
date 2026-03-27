using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.SwitchGameState(GameStates.Resetting);
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.SwitchGameState(GameStates.Playing);
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.OnCameraToggle?.Invoke();
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Instance.OnTogglePathVisualization?.Invoke();
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.EscapeMenuUI.ToggleEscapeMenu();
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.OnSoundToggle?.Invoke();
            GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Button);
        }
    }
}
