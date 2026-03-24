using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovingState : NpcBaseState
{
    public NpcMovingState(NpcStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.PlayIdleWalkRunAnimation();
        stateMachine.MovementController.Move(
            stateMachine.GolfBalls.Count > 0
                ? stateMachine.GolfBalls[0].transform.position
                : stateMachine.StartPoint.position);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.AnimationController.UpdateIdleWalkRunRatio(1f, deltaTime);

        if (stateMachine.MovementController.IsInCollectRange())
        {
            stateMachine.SwitchState(new NpcCollectState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
