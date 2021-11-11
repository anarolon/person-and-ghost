using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersonAndGhost.Ghost
{
    [RequireComponent(typeof(GhostPossession))]
    public class GhostAnimationController : MonoBehaviour
    {
        private GhostPossession possessionComponent;
        private Animator anim;

        private void Start()
        {
            possessionComponent = GetComponent<GhostPossession>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            bool possessing = possessionComponent.IsPossessing;
            anim.SetBool("isPossessing", possessing);
        }
    }
}
