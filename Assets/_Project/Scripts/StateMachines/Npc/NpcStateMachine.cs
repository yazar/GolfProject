using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcStateMachine : StateMachine
{
    [field: Header("Settings")]
    [field: SerializeField] public NpcSettings NpcSettings { get; private set; }
    [field: SerializeField] public Transform StartPoint { get; private set; }
    
    [field: Header("Controllers")]
    [field: SerializeField] public MovementController MovementController { get; private set; }
    [field: SerializeField] public AnimationController AnimationController { get; private set; }
    

    public List<GolfBall> _targetGolfBalls = new List<GolfBall>();
    public List<Vector3> _pathPositions = new List<Vector3>();
    private float _totalPoints = 0;
    private float _pointsOfBallsOnNpc = 0;
    
    private void Awake()
    {
        transform.position = StartPoint.position;
        SwitchState(new NpcIdleState(this));
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPlay += HandlePlay;
        GameManager.Instance.OnReset += HandleReset;
        GameManager.Instance.OnBallsScattered += HandleBallsScattered;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlay -= HandlePlay;
        GameManager.Instance.OnReset -= HandleReset;
        GameManager.Instance.OnBallsScattered -= HandleBallsScattered;
    }
    
    private void HandlePlay()
    {
        if (_pathPositions.Count == 0)
        {
            Debug.LogWarning("There are no balls in reach of health");
            GameManager.Instance.SwitchGameState(GameStates.WaitingForPlay);
            return;
        }
        SwitchState(new NpcMovingState(this));
    }
    private void HandleReset()
    {
        transform.position = StartPoint.position;
        AnimationController.UpdateLocomotionRatio(0f);
        SwitchState(new NpcIdleState(this));
    }
    
    private void HandleBallsScattered(List<GolfBall> golfBalls)
    {
        (_pathPositions, _targetGolfBalls) = PathCalculator.CalculatedPath(StartPoint, golfBalls, NpcSettings);
    }

    #region PublicAccessors
    public IReadOnlyList<GolfBall> TargetGolfBalls => _targetGolfBalls;
    public IReadOnlyList<Vector3> PathPositions => _pathPositions;
    
    #endregion PublicAccessors

    #region PublicFunctions

    public void ReachedNextPosition()
    {
        if (_pathPositions.Count == 0)
        {
            Debug.LogWarning("No path positions found");
            return;
        }
        
        _pathPositions.RemoveAt(0);

        bool isDropOff = _targetGolfBalls[0] is null;

        if(isDropOff)
        {
            _targetGolfBalls.RemoveAt(0);
            SwitchState(new NpcDropBallsState(this));
        }
        else
        {
            _pointsOfBallsOnNpc += _targetGolfBalls[0].GetBallPoints();
            _targetGolfBalls.RemoveAt(0);
            SwitchState(new NpcCollectState(this));
        }
    }

    public void DroppedBalls()
    {
        _totalPoints += _pointsOfBallsOnNpc;
        _pointsOfBallsOnNpc = 0f;
        
        if (_pathPositions.Count == 0)
        {
            SwitchState(new NpcIdleState(this));
            GameManager.Instance.SwitchGameState(GameStates.Completed);
        }
        else
        {
            SwitchState(new NpcMovingState(this));
        }
    }
    
    #endregion PublicFunctions

}
