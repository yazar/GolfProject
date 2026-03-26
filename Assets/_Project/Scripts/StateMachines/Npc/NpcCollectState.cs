using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCollectState : NpcBaseState
{
    public NpcCollectState(NpcStateMachine stateMachine) : base(stateMachine) { }

    private float _enterTime;
    
    public override void Enter()
    {
        _enterTime = Time.time;

        stateMachine.AnimationController.PlayPickUpAnimation();
    }

    public override void Tick(float deltaTime)
    {
        if (Time.time - _enterTime > stateMachine.NpcSettings.pickUpDuration)
        {
            stateMachine.SwitchState(new NpcMovingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
