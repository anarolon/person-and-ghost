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
        this.rb = GetComponent<Rigidbody2D>();
        this.stateMachine = new AIStateMachine(this);
        this.stateMachine.RegisterState(new AIPossessedState());
    }

    public virtual void FixedUpdate()
    {
        this.stateMachine.UpdateState();
    }

    public virtual IEnumerator StateLoop() {
        yield return new WaitUntil(() => this.stateMachine.currentState == this.initialState);
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
