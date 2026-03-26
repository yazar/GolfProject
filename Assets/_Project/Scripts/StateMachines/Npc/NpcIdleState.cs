using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcBaseState
{
    public NpcIdleState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.MovementController.Stop();

        stateMachine.AnimationController.PlayLocomotionAnimation();
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
