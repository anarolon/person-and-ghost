using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBigBoy : AIAgent
{
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float movableDistance = 0.5f;
    GameObject movableItem;
    public bool isGrabbing = false;

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
        while(stateMachine.currentState != AIStateId.Possessed) 
        {
            this.stateMachine.ChangeState(AIStateId.Idle);
            yield return new WaitForSeconds(this.config._doIdleFrequency);
            this.xDirection *= -1;
            if (stateMachine.currentState != AIStateId.Possessed)
            {
                this.stateMachine.ChangeState(AIStateId.MoveX);
                yield return new WaitForSeconds(this.config._changeDirectionFrequency);
            }
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

    private IEnumerator Grab() 
    {   
        Physics2D.queriesStartInColliders = false;
        // Might need to replace transform.right with (Vector2.right * transform.localScale.x)
        // once directions are fixed for Ghost (?) 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, movableDistance, _interactableLayer);
        
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Movable") ) 
        {
            movableItem = hit.collider.gameObject;
            movableItem.GetComponent<FixedJoint2D>().enabled = true;
            movableItem.GetComponent<Movable>().isBeingPushed = true;
            movableItem.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            isGrabbing = true;
        }
        
        yield return null;
    }

    public override void StolenAction(Vector2 direction = default) 
    {
        if(isGrabbing)
        {
            StopCoroutine(Grab());
            movableItem.GetComponent<FixedJoint2D>().enabled = false;
            movableItem.GetComponent<Movable>().isBeingPushed = false;
            isGrabbing = false;
        }
        else
        {
            StartCoroutine(Grab());
        }
    }
}
