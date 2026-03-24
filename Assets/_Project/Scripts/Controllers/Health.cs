using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private NpcSettings npcSettings;
    
    public float _health;
    
    private bool _isPlaying;

    private void Start()
    {
        HandleReset();
    }

    private void HandleReset()
    {
        _isPlaying = false;
        _health =  npcSettings.healthDuration;
    }
    
    private void HandlePlay()
    {
        _isPlaying = true;
    }

    private void Update()
    {
        if(!_isPlaying) return;
        
        _health -= Time.deltaTime;

        if (_health <= 0f)
        {
            _isPlaying = false;
            GameManager.Instance.SwitchGameState(GameStates.Death);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReset += HandleReset;
        GameManager.Instance.OnPlay += HandlePlay;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReset -= HandleReset;
        GameManager.Instance.OnPlay -= HandlePlay;
    }
}
