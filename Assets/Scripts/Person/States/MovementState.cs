using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PersonState
{
    private Vector2 movementInput;
    private bool jumped;

    private bool meditating;

    public MovementState(PersonController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        movementInput = character.MovementInput;
        jumped = character.Jumped;

        meditating = character.IsMeditating;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(character.idle);
        }
        else if (jumped)
        {
            stateMachine.ChangeState(character.jumping);
        }
        else if (meditating)
        {
            stateMachine.ChangeState(character.meditate);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move();

    }
}
