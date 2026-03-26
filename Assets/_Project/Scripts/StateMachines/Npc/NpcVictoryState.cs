using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVictoryState : NpcBaseState
{
    public NpcVictoryState(NpcStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.AnimationController.PlayVictoryAnimation();
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
