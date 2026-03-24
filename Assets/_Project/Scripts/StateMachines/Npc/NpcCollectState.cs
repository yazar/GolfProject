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
        
        stateMachine.GolfBalls.RemoveAt(0);
        _startTime = Time.time;
    }

    public override void Tick(float deltaTime)
    {
        if (Time.time - _startTime > 0.5f)
        {
            stateMachine.SwitchState(new NpcMovingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
