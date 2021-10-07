using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Tools
{
    public class PersonToolController : MonoBehaviour
    {
        [SerializeField] private bool toolInHand = false;

        [Header("Input System Variables")]
        //[SerializeField] private bool interactWithTool = false;
        [SerializeField] private float interactWithTool = 0;
        [SerializeField] private bool useAction = false;

        void Start()
        {
            toolInHand = false;
        }

        void Update()
        {
            if (interactWithTool < 0 && toolInHand)
            {
                DropTool();
            }

            if (useAction && toolInHand)
            {
                Actions.OnToolActionUse(gameObject);
            }
        }

        public void OnToolInteract(InputAction.CallbackContext context)
        {
            //interactWithTool = context.action.triggered;
            interactWithTool = context.ReadValue<float>();
        }

        public void OnToolUse(InputAction.CallbackContext context)
        {
            useAction = context.action.triggered;
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (interactWithTool > 0 && collision.CompareTag("Tool") && !toolInHand)
            {
                PickupTool();
            }
        }


        private void PickupTool()
        {
            Actions.OnToolPickup(gameObject);

            toolInHand = true;
        }

        private void DropTool()
        {
            Actions.OnToolDrop(gameObject);

            toolInHand = false;
        }
    }
}