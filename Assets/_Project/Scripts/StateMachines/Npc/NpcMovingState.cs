using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovingState : NpcBaseState
{
    public NpcMovingState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.PlayIdleWalkRunAnimation();

        stateMachine.MovementController.Move(stateMachine.PathPositions[0]);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.MovementController.GetRemainingDistance() < 0.6f)
        {
            stateMachine.PathPositions.RemoveAt(0);
            stateMachine.MovementController.Stop();
            
            if(stateMachine.PathPositions.Count > 0)
            {
                stateMachine.SwitchState(new NpcCollectState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new NpcIdleState(stateMachine));
            }            
            return;
        }
        
        stateMachine.AnimationController.UpdateIdleWalkRunRatio(1f, deltaTime);
    }

    public override void Exit()
    {
        
    }
}
