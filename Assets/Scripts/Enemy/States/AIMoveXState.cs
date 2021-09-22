using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveXState : AIState
{ 
    public AIStateId GetId()
    {
        return AIStateId.MoveX;
    }
    public void Enter(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        agent.rb.drag = agent.config._linearDrag;
        Vector2 movement = new Vector2(agent.xDirection, 0f);
        movement *= agent.config._movementAcceleration;
        agent.rb.AddForce(movement);

        var velocity = agent.rb.velocity;
        if (Mathf.Abs(velocity.x) > agent.config._maxMoveSpeed)
        {
            var x = Mathf.Sign(velocity.x) * agent.config._maxMoveSpeed;
            var y = velocity.y;
            agent.rb.velocity = new Vector2(x, y);
        }

    }
}
