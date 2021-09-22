using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }
    public void Enter(AIAgent agent)
    {
        //Debug.Log("I'm Idle.");
    }

    public void Exit(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {

    }
}
