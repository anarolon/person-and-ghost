using System.Collections;
using System.Collections.Generic;
using PersonAndGhost.Person;
using PersonAndGhost.Person.States;
using UnityEngine;

public class MeditatingState : PersonState
{
    private bool _meditating;

    [Header("Minigame fields")]
    private GameObject _ghostlyInvasion;

    public MeditatingState(PersonMovement character, StateMachine stateMachine) : base(character, stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        _ghostlyInvasion = GameObject.Instantiate(character.GhostlyInvasion);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        _meditating = character.IsMeditating;

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
            GameObject.Destroy(_ghostlyInvasion);
        }

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }


}
