using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovingState : NpcBaseState
{
    public NpcMovingState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.PathPositions.Count == 0)
        {
            Debug.LogWarning("Entered Moving State Without Any Path Positions. Exiting to Idle");
            stateMachine.SwitchState(new NpcIdleState(stateMachine));
            return;
        }
        
        stateMachine.AnimationController.PlayLocomotionAnimation();

        stateMachine.MovementController.Move(stateMachine.PathPositions[0]);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.MovementController.GetRemainingDistance() < stateMachine.NpcSettings.stoppingDistance)
        {
            stateMachine.ReachedNextPosition();
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.MovementController.Stop();
    }
}
