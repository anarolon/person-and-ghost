using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PersonState
{
    private Vector2 movementInput;
    private bool jumped;

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

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move();

    }
}
