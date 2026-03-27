using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private NpcSettings npcSettings;
    
    private float _health;
    
    private bool _isPlaying;

    private void Start()
    {
        HandleReset();
        GameManager.Instance.HealthBarUI.SetFillAmount(1f);
    }
    
    private void Update()
    {
        if(!_isPlaying) return;
        
        _health -= Time.deltaTime;

        float ratio = Mathf.Max(0f, _health / npcSettings.healthDuration);
        GameManager.Instance.HealthBarUI.SetFillAmount(ratio);


        if (_health <= 0f)
        {
            _isPlaying = false;
            GameManager.Instance.SwitchGameState(GameStates.Lose);
        }
        
    }

    private void HandleReset()
    {
        _isPlaying = false;
        _health =  npcSettings.healthDuration;
        GameManager.Instance.HealthBarUI.SetFillAmount(1f);
    }
    
    private void HandlePlay()
    {
        _isPlaying = true;
    }
    
    private void HandleCompleted()
    {
        _isPlaying = false;
    }
    
    private void HandleWaitingForPlay()
    {
        _isPlaying = false;
        _health =  npcSettings.healthDuration;
        GameManager.Instance.HealthBarUI.SetFillAmount(1f);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReset += HandleReset;
        GameManager.Instance.OnPlay += HandlePlay;
        GameManager.Instance.OnRouteCompleted += HandleCompleted;
        GameManager.Instance.OnWaitingForPlay += HandleWaitingForPlay;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReset -= HandleReset;
        GameManager.Instance.OnPlay -= HandlePlay;
        GameManager.Instance.OnRouteCompleted -= HandleCompleted;
        GameManager.Instance.OnWaitingForPlay -= HandleWaitingForPlay;
    }
}