using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [field: SerializeField] public HealthBarUI HealthBarUI { get; private set; }
    [field: SerializeField] public PointsUI PointsUI { get; private set; }
    [field: SerializeField] public PointsUI PointsWaitingUI { get; private set; }
    [field: SerializeField] public SettingsMenuUI SettingsMenuUI { get; private set; }
    [field: SerializeField] public EscapeMenuUI EscapeMenuUI { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }
    
    private GameStates _currentGameState;
    
    public Action OnPlay;
    public Action OnReset;
    public Action OnLose;
    public Action OnRouteCompleted;
    public Action OnWaitingForPlay;
    public Action OnTogglePathVisualization;
    public Action OnCameraToggle;
    public Action<CameraType> OnCameraChange;
    public Action<List<GolfBall>> OnBallsScattered;
    public Action<float> OnCollectedBall;
    public Action OnBallsDropped;
    public Action OnSoundToggle;


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
