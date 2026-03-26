using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLoseState : NpcBaseState
{
    public NpcLoseState(NpcStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.AnimationController.PlayLoseAnimation();
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
