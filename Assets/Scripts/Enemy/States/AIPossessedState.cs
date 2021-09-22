using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPossessedState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Possessed;
    }
    public void Enter(AIAgent agent)
    {
        agent.isPossessed = true;
        agent.StopCoroutine(agent.StateLoop());
    }
    public void Update(AIAgent agent)
    {

    }
    
    public void Exit(AIAgent agent)
    {
        agent.isPossessed = false;
        agent.StartCoroutine(agent.StateLoop());
    }
}
