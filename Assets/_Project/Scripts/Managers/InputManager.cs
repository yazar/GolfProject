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
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.SwitchGameState(GameStates.Playing);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.OnCameraToggle?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Instance.OnTogglePathVisualization?.Invoke();
        }
    }
}
