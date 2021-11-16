using System.Collections;
using System.Collections.Generic;
using PersonAndGhost.Person;
using PersonAndGhost.Person.States;
using UnityEngine;

namespace PersonAndGhost
{
    public class PersonAnimationController : MonoBehaviour
    {   
        private Animator animator;
        private PersonMovement personController;
        private const string IDLE_STATE = "IdleState";
        private const string JUMPING_STATE = "JumpingState";
        private const string FALLING_STATE = "FallingState";
        private const string MEDITATING_STATE = "MediditatingState";
        private const string MOVING_STATE = "MovementState";
        private const string CLING_STATE = "ClingState";
        private const string DEATH_STATE = "DeathState";

        void Start()
        {
            this.animator = GetComponent<Animator>();
            this.personController = GetComponent<PersonMovement>();
        }

        void Update()
        {
            // Animation transitions should be optimized in future work
            string currentState = personController.MovementSM.CurrentState.StateId();
            
            animator.SetBool("isJumping", personController.Jumped);
            animator.SetBool("isDead", personController.isDead);
            
            if(personController.IsOnGround) {
                animator.SetBool("isFalling", false);
                animator.SetBool("isClinging", false);
            } else {
                 animator.SetBool("isFalling", true);
            }

            if(currentState == FALLING_STATE) {
                animator.SetBool("isFalling", true);
            } 

            if(currentState == IDLE_STATE) {
                animator.SetBool("isFalling", false);
                animator.SetBool("isClinging", false);
            }

            if(currentState == CLING_STATE) {
                animator.SetBool("isClinging", true);
            } else {
                animator.SetBool("isClinging", false);
            }

            if(currentState == MOVING_STATE) {
                animator.SetBool("isMoving", true);
            } else {
                animator.SetBool("isMoving", false);
            }

            if(personController.IsMeditating) {
                animator.SetBool("isMeditating", true);
            } else {
                animator.SetBool("isMeditating", false);
            }
        }
    }
}
