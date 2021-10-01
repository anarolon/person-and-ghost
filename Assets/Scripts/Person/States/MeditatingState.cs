using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditatingState : PersonState
{
    private Vector2 movementInput;
    private bool jumped;
    private bool meditating;


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
        meditating = character.IsMeditating;

        movementInput = character.MovementInput;
        jumped = character.Jumped;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!meditating)
        {
            stateMachine.ChangeState(character.idle);
        }
        

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (movementInput.x != 0)
        {
            // TODO: Move brainwave laser
            Debug.Log("laser move!");
        }
        else if (jumped)
        {

            // TODO: shoot laster on jump
            Debug.Log("laser shoot!");

        }
    }


}
