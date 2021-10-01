using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PersonState
{
    private bool meditating;
    public JumpingState(PersonController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.Jump();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        meditating = character.IsMeditating;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.IsOnGround)
        {
            stateMachine.ChangeState(character.idle);
        }
        else if (meditating)
        {
            stateMachine.ChangeState(character.meditate);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMove();
    }
}
