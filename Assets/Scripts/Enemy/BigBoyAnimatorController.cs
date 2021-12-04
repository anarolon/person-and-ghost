using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersonAndGhost
{

    [RequireComponent(typeof(AIBigBoy))]
    [RequireComponent(typeof(Animator))]
    public class BigBoyAnimatorController : MonoBehaviour
    {
        private Animator anim;
        private AIBigBoy bigBoyComponent;

        private string actionParameter = "isGrabbing";
        private string moveParameter = "isMoving";

        private void Start()
        {
            bigBoyComponent = GetComponent<AIBigBoy>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            anim.SetBool(actionParameter, bigBoyComponent.isGrabbing);
            anim.SetBool(moveParameter, bigBoyComponent.rb.velocity != Vector2.zero);
        }
    }
}
