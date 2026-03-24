using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcBaseState
{
    public NpcIdleState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.PlayIdleWalkRunAnimation();
        SetTargetMovePosition(stateMachine.transform.position);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.AnimationController.UpdateIdleWalkRunRatio(0f, deltaTime);
    }

    public override void Exit()
    {
        
    }
}
