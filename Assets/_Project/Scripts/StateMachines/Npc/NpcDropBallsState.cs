using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDropBallsState : NpcBaseState
{
    public NpcDropBallsState(NpcStateMachine stateMachine) : base(stateMachine) { }
    
    private float _enterTime;

    public override void Enter()
    {
        _enterTime = Time.time;

        stateMachine.AnimationController.PlayDropAnimation();
    }

    public override void Tick(float deltaTime)
    {
        if (Time.time - _enterTime > stateMachine.NpcSettings.dropOffDuration)
        {
            stateMachine.DroppedBalls();
        }
    }

    public override void Exit()
    {
        
    }
}
