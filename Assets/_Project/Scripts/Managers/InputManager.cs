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
    }
}
