using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFrog : AIAgent
{
    public override void Start()
    {
        base.Start();
        this.stateMachine.RegisterState(new AIJumpState());
        this.stateMachine.ChangeState(AIStateId.Jump);
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
            _isJumping = true;
            this.xDirection *= -1;
            stateMachine.ChangeState(AIStateId.Jump);
            yield return new WaitForSeconds(this.config._doJumpFrequency);
        }

        _isJumping = false;
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

        if (CanJump)
        {
            this.rb.drag = this.config._airLinearDrag;
            this.rb.velocity = new Vector2(this.rb.velocity.x, 0f);
            this.rb.AddForce(Vector2.up * this.config._jumpForce, ForceMode2D.Impulse);
            this._isJumping = false;
        }
    }

    public override void OnCollisionEnter2D(Collision2D other) 
    {
        if(Equals(other.gameObject.tag, "Ground")) {
            _isGrounded = true;
        }
    }

    public override void OnCollisionExit2D(Collision2D other) 
    {
        if(Equals(other.gameObject.tag, "Ground")) {
            _isGrounded = false;
        }
    }
}
