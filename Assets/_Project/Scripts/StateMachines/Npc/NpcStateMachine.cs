using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine : StateMachine
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public MovementController MovementController { get; private set; }
    [field: SerializeField] public AnimationController AnimationController { get; private set; }

    private List<Transform> _golfBalls = new List<Transform>();
    private List<Vector3> _pathPositions = new List<Vector3>();
    public List<Transform> GolfBalls => _golfBalls;
    public List<Vector3> PathPositions => _pathPositions;
    
    private void Awake()
    {
        transform.position = StartPoint.position;
        SwitchState(new NpcIdleState(this));
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReset += HandleReset;
        GameManager.Instance.OnBallsScattered += HandleBallsScattered;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReset -= HandleReset;
        GameManager.Instance.OnBallsScattered -= HandleBallsScattered;
    }
    
    private void HandleReset()
    {
        transform.position = StartPoint.position;
        SwitchState(new NpcIdleState(this));
    }
    
    private void HandleBallsScattered(List<Transform> golfBalls)
    {
        
        _golfBalls = golfBalls;
        _pathPositions = PathCalculator.CalculatedPath(StartPoint, _golfBalls);
        SwitchState(new NpcMovingState(this));
    }

}
