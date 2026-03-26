using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCollectState : NpcBaseState
{
    public NpcCollectState(NpcStateMachine stateMachine) : base(stateMachine) { }

    private float _enterTime;
    private bool _collectedBall;
    
    public override void Enter()
    {
        _enterTime = Time.time;

        stateMachine.AnimationController.PlayPickUpAnimation();
    }

    public override void Tick(float deltaTime)
    {
        if (!_collectedBall && Time.time - _enterTime > stateMachine.NpcSettings.pickUpDuration / 2f)
        {
            stateMachine.CollectedBall();
            _collectedBall = true;
        }
        
        if (Time.time - _enterTime > stateMachine.NpcSettings.pickUpDuration)
        {
            stateMachine.SwitchState(new NpcMovingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
