using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    private GameStates _currentGameState;
    
    public Action OnPlay;
    public Action OnReset;
    public Action OnLose;
    public Action OnRouteCompleted;
    public Action OnWaitingForPlay;
    public Action<List<GolfBall>> OnBallsScattered;


    public void SwitchGameState(GameStates targetState)
    {
        if (_currentGameState == targetState)
        {
            Debug.LogWarning("Trying to enter the same state. Already in state : " + targetState);
            return;
        }
        
        switch (targetState)
        {
            case GameStates.WaitingForPlay:
                _currentGameState = targetState;
                OnWaitingForPlay?.Invoke();
                break;
            case GameStates.Playing:
                if(_currentGameState != GameStates.WaitingForPlay)
                {
                    break;
                }
                _currentGameState = targetState;
                OnPlay?.Invoke();
                break;
            case GameStates.Lose:
                _currentGameState = targetState;
                OnLose?.Invoke();
                break;
            case GameStates.Resetting:
                _currentGameState = targetState;
                OnReset?.Invoke();
                break;
            case GameStates.Completed:
                _currentGameState = targetState;
                OnRouteCompleted?.Invoke();
                break;
        }
        
    }
    

    #region Singleton
    
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    #endregion
    
}

public enum GameStates
{
    None,
    WaitingForPlay,
    Playing,
    Lose,
    Resetting,
    Completed,
}
