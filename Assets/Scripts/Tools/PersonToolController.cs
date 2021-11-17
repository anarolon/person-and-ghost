using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Utils;

namespace PersonAndGhost.Tools
{
    public class PersonToolController : MonoBehaviour
    {
        [SerializeField] private bool toolInHand = false;

        [Header("Input System Variables")]
        [SerializeField] private float interactWithTool = 0;
        [SerializeField] private bool useAction = false;

        GameObject currentTool = default;

        void Start()
        {
            toolInHand = false;
        }

        void Update()
        {
            if (interactWithTool < 0 && toolInHand)
            {
                DropTool(currentTool);
            }

            if (useAction && toolInHand)
            {
                Actions.OnToolActionUse(gameObject);
            }
        }

        public void OnToolInteract(InputAction.CallbackContext context)
        {
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
                PickupTool(collision.gameObject);
            }
        }


        private void PickupTool(GameObject tool)
        {
            Actions.OnToolPickup(gameObject, tool);
            currentTool = tool;
            toolInHand = true;
        }

        private void DropTool(GameObject tool)
        {
            Actions.OnToolDrop(gameObject, tool);

            toolInHand = false;
        }
    }
}