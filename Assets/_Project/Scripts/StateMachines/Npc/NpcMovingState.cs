using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovingState : NpcBaseState
{
    public NpcMovingState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.PlayLocomotionAnimation();

        stateMachine.MovementController.Move(stateMachine.PathPositions[0]);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.MovementController.GetRemainingDistance() < 0.6f)
        {
            stateMachine.MovementController.Stop();
            stateMachine.ReachedNextPosition();
            return;
        }
        
        stateMachine.AnimationController.UpdateLocomotionRatio(1f, deltaTime);
    }

    public override void Exit()
    {
        
    }
}
