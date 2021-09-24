using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBird : AIAgent
{
    public override void Start()
    {
        base.Start();
        this.stateMachine.RegisterState(new AIMoveXState());
        stateMachine.ChangeState(AIStateId.MoveX);
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
            stateMachine.ChangeState(AIStateId.MoveX);
            this.xDirection *= -1;
            yield return new WaitForSeconds(this.config._changeDirectionFrequency);
        }
    }

    public override void movementBehaviour(Vector2 movementInput)
    {   
        base.movementBehaviour(movementInput);
        movementInput *= this.config._movementAcceleration;

        this.rb.AddForce(movementInput);

        var velocity = this.rb.velocity;
        if (Mathf.Abs(velocity.x) > this.config._maxMoveSpeed || Mathf.Abs(velocity.y) > this.config._maxMoveSpeed ) 
        {
            var x = Mathf.Sign(velocity.x) * this.config._maxMoveSpeed;
            var y = Mathf.Sign(velocity.y) * this.config._maxMoveSpeed;
            this.rb.velocity = new Vector2(x, y);
        }


        this.rb.drag = this.config._linearDrag;
    }
}
