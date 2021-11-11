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

        private string actionParameter = "IsJumping";

        private void Start()
        {
            frogComponent = GetComponent<AIFrog>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            anim.SetBool(actionParameter, frogComponent._isJumping);
            
        }
    }
}
