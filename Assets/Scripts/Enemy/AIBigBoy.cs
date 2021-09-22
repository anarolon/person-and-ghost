using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBigBoy : AIAgent
{
    
    public override void Start()
    {
        base.Start();
        this.stateMachine.RegisterState(new AIMoveXState());
        StartCoroutine(this.StateLoop());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override IEnumerator StateLoop()
    {
        yield return new WaitUntil(() => stateMachine.currentState == initialState);
        while(stateMachine.currentState != AIStateId.Possessed) {
            this.stateMachine.ChangeState(AIStateId.Idle);
            yield return new WaitForSeconds(this.config._doIdleFrequency);
            this.xDirection *= -1;
            this.stateMachine.ChangeState(AIStateId.MoveX);
            yield return new WaitForSeconds(this.config._changeDirectionFrequency);
        }
    }

    public override void movementBehaviour(Vector2 movementInput)
    {   
        base.movementBehaviour(movementInput);
        Vector2 movement = new Vector2(movementInput.x, 0f);
        movement *= this.config._movementAcceleration;

        this.rb.AddForce(movement);

        var velocity = this.rb.velocity;
        if (Mathf.Abs(velocity.x) > this.config._maxMoveSpeed)
        {
            var x = Mathf.Sign(velocity.x) * this.config._maxMoveSpeed;
            var y = velocity.y;
            this.rb.velocity = new Vector2(x, y);
        }

        this.rb.drag = this.config._linearDrag;
    }
}
