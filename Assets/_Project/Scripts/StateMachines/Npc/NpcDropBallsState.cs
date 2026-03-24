using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDropBallsState : NpcBaseState
{
    public NpcDropBallsState(NpcStateMachine stateMachine) : base(stateMachine) { }
    
    private float _startTime;

    public override void Enter()
    {
        _startTime = Time.time;

        stateMachine.AnimationController.PlayPickUpAnimation();
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.AnimationController.UpdateLocomotionRatio(0f, Time.deltaTime);

        if (Time.time - _startTime > stateMachine.NpcSettings.dropOffDuration)
        {
            stateMachine.DroppedBalls();
        }
    }

    public override void Exit()
    {
        
    }
}
