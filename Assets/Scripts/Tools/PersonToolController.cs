
using UnityEngine;
using UnityEngine.InputSystem;

public class PersonToolController : MonoBehaviour
{
    [SerializeField] private bool toolInHand = false;

    [Header("Input System Variables")]
    [SerializeField] private bool interactWithTool = false;
    [SerializeField] private bool useAction = false;

    void Start()
    {
        toolInHand = false;
    }

    void Update()
    {
        if (interactWithTool && toolInHand)
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
        interactWithTool = context.action.triggered;
    }

    public void OnToolUse(InputAction.CallbackContext context)
    {
        useAction = context.action.triggered;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interactWithTool && collision.CompareTag("Tool") && !toolInHand)
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
