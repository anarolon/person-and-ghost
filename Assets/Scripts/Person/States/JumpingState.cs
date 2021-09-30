using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PersonState
{
    public JumpingState(PersonController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.Jump();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.IsOnGround)
        {
            stateMachine.ChangeState(character.idle);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMove();
    }
}
