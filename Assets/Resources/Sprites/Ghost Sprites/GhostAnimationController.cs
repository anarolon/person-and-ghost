using PersonAndGhost.Utils;
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

        private void OnEnable()
        {
            Actions.OnRoomStateChange += HandleRoomStateChange;
        }

        private void OnDisable()
        {
            Actions.OnRoomStateChange -= HandleRoomStateChange;
        }

        private void HandleRoomStateChange(bool hasWon)
        {
            if (hasWon)
            {
                anim.SetTrigger("RoomWon");
            }
        }
    }
}
