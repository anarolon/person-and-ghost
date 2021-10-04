using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditatingState : PersonState
{
    private Vector2 _movementInput;
    private bool _jumped;
    private bool _meditating;

    public MeditatingState(PersonController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        _meditating = character.IsMeditating;

        _movementInput = character.MovementInput;
        _jumped = character.Jumped;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_meditating)
        {
            if (character.IsOnGround)
            {
               stateMachine.ChangeState(character.idle);
            }
            else
            {
                stateMachine.ChangeState(character.falling);
            }
            
        }
        

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (_movementInput.x != 0)
        {
            // TODO: Move brainwave laser
            Debug.Log("laser move!");
        }
        else if (_jumped)
        {

            // TODO: shoot laster on jump
            Debug.Log("laser shoot!");

        }
    }


}
