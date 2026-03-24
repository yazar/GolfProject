using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCollectState : NpcBaseState
{
    public NpcCollectState(NpcStateMachine stateMachine) : base(stateMachine) { }

    private float _startTime;
    
    public override void Enter()
    {
        if(stateMachine.GolfBalls.Count <= 0)
        {
            stateMachine.SwitchState(new NpcIdleState(stateMachine));
            return;
        }
        
        _startTime = Time.time;

        stateMachine.AnimationController.PlayPickUpAnimation();
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.AnimationController.UpdateIdleWalkRunRatio(0f, Time.deltaTime);

        if (Time.time - _startTime > 2f)
        {
            stateMachine.SwitchState(new NpcMovingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
