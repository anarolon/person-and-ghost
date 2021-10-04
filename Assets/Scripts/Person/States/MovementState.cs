using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PersonState
{
    private Vector2 _movementInput;
    private bool _jumped;

    private bool _meditating;

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
        _movementInput = character.MovementInput;
        _jumped = character.Jumped;

        _meditating = character.IsMeditating;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(character.idle);
        }
        else if (_jumped)
        {
            stateMachine.ChangeState(character.jumping);
        }
        else if (_meditating)
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
