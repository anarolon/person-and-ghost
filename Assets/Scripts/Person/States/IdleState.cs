using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PersonState
{
    private Vector2 movementInput;
    private bool jumped;

    private bool meditating;

    public IdleState(PersonController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.ResetVelocity();
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
        if (movementInput.x != 0)
        {
            stateMachine.ChangeState(character.movement);
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

}
