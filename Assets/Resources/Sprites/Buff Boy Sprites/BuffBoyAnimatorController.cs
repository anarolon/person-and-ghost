using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersonAndGhost
{
    [RequireComponent(typeof(AIBuffBoy))]
    [RequireComponent(typeof(Animator))]
    public class BuffBoyAnimatorController : MonoBehaviour
    {
        private Animator anim;
        private AIBuffBoy buffBoyComponent;

        private string punchParameter = "Punch";
        private string stompParameter = "Stomp";
        private string moveParameter = "IsMoving";

        private void Start()
        {
            buffBoyComponent = GetComponent<AIBuffBoy>();
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            anim.SetBool(moveParameter, buffBoyComponent.rb.velocity != Vector2.zero);
            if (buffBoyComponent.Punch)
            {
                anim.SetTrigger(punchParameter);
            }
            else if (buffBoyComponent.Stomp)
            {
                anim.SetTrigger(stompParameter);
            }
            
        }
    }
}
