using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersonAndGhost
{
    [RequireComponent(typeof(AIFrog))]
    [RequireComponent(typeof(Animator))]
    public class FrogAnimatorController : MonoBehaviour
    {
        private Animator anim;
        private AIFrog frogComponent;

        private string actionParameter = "Jumped";
        private string movementParameter = "IsMoving";

        private void Start()
        {
            frogComponent = GetComponent<AIFrog>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (frogComponent._isJumping) anim.SetTrigger(actionParameter);
            anim.SetBool(movementParameter, frogComponent.rb.velocity.x != 0);
            
        }
    }
}
