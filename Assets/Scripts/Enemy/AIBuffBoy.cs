using System.Collections;
using System.Collections.Generic;
using PersonAndGhost.Utils;
using UnityEngine;

public class AIBuffBoy : AIAgent
{
    [SerializeField] private Transform _smashPoint;
    [SerializeField] private Transform _stompPoint;
    [SerializeField] private LayerMask _breakableLayer;
    [SerializeField] private float breakableDistance = 0.5f;

    public bool Punch;
    public bool Stomp;

    public override void Start()
    {
        base.Start();
        this.stateMachine.RegisterState(new AIMoveXState());
        StartCoroutine(this.StateLoop());
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Punch = false;
        Stomp = false;
    }

    public override IEnumerator StateLoop()
    {
        yield return new WaitUntil(() => stateMachine.currentState == initialState);
        while(stateMachine.currentState != AIStateId.Possessed) {
            this.stateMachine.ChangeState(AIStateId.Idle);
            yield return new WaitForSeconds(this.config._doIdleFrequency);
        }
    }
    
    public override void movementBehaviour(Vector2 movementInput){
        //base.movementBehaviour(movementInput);
        if (movementInput == Vector2.left)
        {
            transform.eulerAngles = new Vector2(0,180);
        }
        else if (movementInput == Vector2.right)
        {
            transform.eulerAngles = new Vector2(0,0);
        }
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

    private IEnumerator Break(Transform firePoint) 
    {
        RaycastHit2D  hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, breakableDistance, _breakableLayer);

        //TODO: Magic Number (This is how long the animation lasts)
        yield return new WaitForSeconds(0.2f);

        if (hitInfo)
        {
            Breakable breakable = hitInfo.transform.GetComponent<Breakable>();
            if (breakable != null)
            {
                breakable.Break();
            }
        }
    }

    public override void StolenAction(Vector2 direction = default) {
        StartCoroutine(Break(direction==default? _smashPoint : _stompPoint));
        // TODO: For some reason punch direction is always detected
        // The if statement always goes to the Punch block of code if punch is the first if
        // because of this, I moved it to the second if, but we should check out why Punch is always detected
        if (direction == Vector2.down)
        {
            Stomp = true;
            Debug.Log("Order to Stomp");
        }
        else
        {
            Punch = true;
            Debug.Log("Order to Punch");
        }
    }
}
