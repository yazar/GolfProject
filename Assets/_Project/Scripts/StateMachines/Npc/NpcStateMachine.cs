using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine : StateMachine
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public MovementController MovementController { get; private set; }
    [field: SerializeField] public AnimationController AnimationController { get; private set; }

    private List<Transform> _golfBalls = new List<Transform>();
    public List<Transform> GolfBalls => _golfBalls;
    
    private void Start()
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
    
    private void HandleBallsScattered(List<Transform> balls)
    {
        _golfBalls = balls;
        
        SwitchState(new NpcMovingState(this));
    }

}
