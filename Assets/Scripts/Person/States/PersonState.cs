using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonState : State
{
    protected PersonController character;
    public PersonState(PersonController character, StateMachine stateMachine) : base(stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }

}
