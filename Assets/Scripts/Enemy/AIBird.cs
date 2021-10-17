using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PersonAndGhost.Utils;

public class AIBird : AIAgent
{
    public bool isCarrying = false;
    private AIBird_Claws claws = default;

    public override void Start()
    {
        base.Start();
        this.stateMachine.RegisterState(new AIMoveXState());
        stateMachine.ChangeState(AIStateId.MoveX);
        StartCoroutine(this.StateLoop());
        claws = gameObject.GetComponentInChildren<AIBird_Claws>();
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

        // Execute Stolen Action when Person is near Bird's claws 
        // and the designated Stolen Action key is pressed. 
        if (claws.isPersonNearby && CanAct)
        {
            ClawAction();
            this._isActing = false;
        }
    }

    public void ClawAction()
    {
        // When Person is currently being carried by Bird, drop them.
        if(isCarrying)
        {      
            claws.Drop();
            isCarrying = false;
        }
        // When Person is not being carried by Bird, pick them up.
        else
        {
            claws.PickUp();
            isCarrying = true;
        }

    }
    
}
