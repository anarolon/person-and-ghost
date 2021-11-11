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
        Punch = false;
        Stomp = false;
        base.FixedUpdate();
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

    private IEnumerator Break(Transform firePoint) 
    {
        RaycastHit2D  hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, breakableDistance, _breakableLayer);

        if (hitInfo)
        {
            Breakable breakable = hitInfo.transform.GetComponent<Breakable>();
            if(breakable != null) 
            {
                breakable.Break();
            }
        }
        
        yield return null;
    }

    public override void StolenAction(Vector2 direction = default) {
        if (direction == default)
            Punch = true;
        else
            Stomp = true;
        StartCoroutine(Break(direction==default? _smashPoint : _stompPoint));
    }
}
