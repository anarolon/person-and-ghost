using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initialState;
    public AIAgentConfig config;
    public Rigidbody2D rb;    
    public bool isPossessed = false;
    public float xDirection = 1;
    public bool _isJumping, _isGrounded, _isActing;
    public bool CanJump => _isJumping && _isGrounded;
    public bool CanAct => _isActing && isPossessed;

    private SpriteRenderer _monsterSprite;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIPossessedState());
        stateMachine.RegisterState(new AIIdleState());

        _monsterSprite = GetComponent<SpriteRenderer>();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.UpdateState();
        if(_monsterSprite) _monsterSprite.flipX = xDirection < 0;
    }

    public virtual IEnumerator StateLoop() {
        yield return null;
    }

    public virtual void movementBehaviour(Vector2 movementInput){
        if(movementInput == Vector2.left) {
            //transform.eulerAngles = new Vector2(0,180);
            xDirection = -1;
        } else if(movementInput == Vector2.right) {
            //transform.eulerAngles = new Vector2(0,0);
            xDirection = 1;
        }
    }

    public virtual void StolenAction(Vector2 direction = default) {

    }

    public virtual void OnCollisionEnter2D(Collision2D other) 
    {
        
    }

    public virtual void OnCollisionExit2D(Collision2D other) 
    {
        
    }
}
