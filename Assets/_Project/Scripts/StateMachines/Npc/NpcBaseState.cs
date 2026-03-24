using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBaseState : State
{
    protected NpcStateMachine stateMachine;
    public NpcBaseState(NpcStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
