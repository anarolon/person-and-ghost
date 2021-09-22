using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Jump;
    }
    public void Enter(AIAgent agent)
    {
       
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        if(agent.CanJump){
            agent.rb.drag = agent.config._airLinearDrag;
            agent.rb.velocity = new Vector2(agent.xDirection * agent.config._maxMoveSpeed, 0f);
            agent.rb.AddForce(Vector2.up * agent.config._jumpForce, ForceMode2D.Impulse);
            agent._isJumping = false;
        }
        
    }

    private void FallMultiplier(AIAgent agent)
    {
        var velocity = agent.rb.velocity.y;
        if (velocity < 0)
        {
            agent.rb.gravityScale = agent.config._fallMultiplier;
        }
        else if (velocity > 0 && !(agent._isJumping))
        {
            agent.rb.gravityScale = agent.config._lowJumpFallMultiplier;
        }
        else
        {
            agent.rb.gravityScale = 1f;
        }
    }


}
