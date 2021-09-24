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
    public bool _isJumping, _isGrounded;
    public bool CanJump => _isJumping && _isGrounded;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIPossessedState());
        stateMachine.RegisterState(new AIIdleState());
    }

    public virtual void FixedUpdate()
    {
        stateMachine.UpdateState();
    }

    public virtual IEnumerator StateLoop() {
        yield return null;
    }

    public virtual void movementBehaviour(Vector2 movementInput){
        
    }

    public virtual void OnCollisionEnter2D(Collision2D other) 
    {
        
    }

    public virtual void OnCollisionExit2D(Collision2D other) 
    {
        
    }
}
