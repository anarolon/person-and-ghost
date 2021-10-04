using UnityEngine;
using UnityEngine.InputSystem;

public class ToolUser : MonoBehaviour
{
    [Header("Object Pickup Prototpye Fields")]
    [SerializeField] private Tool _toolScript = default;
    [SerializeField] private float _pickupRange = 1;

    private Transform _selfTranform;
    private Transform _toolTransform;
    private float _dropForce;

    private bool _equipped;


    [SerializeField] private bool _toolInHand = false;

    [Header("Input System Variables")]
    [SerializeField] private bool _interactWithTool = false;
    [SerializeField] private bool _useAction = false;

    void Start()
    {
        _toolInHand = false;
    }

    void Update()
    {
        if (_interactWithTool && _toolInHand)
        {
            DropTool();
        }

        if (_useAction && _toolInHand)
        {
            Actions.OnToolActionUse(gameObject);
        }

    }

    public void OnToolInteract(InputAction.CallbackContext context)
    {
        _interactWithTool = context.action.triggered;
    }

    public void OnToolUse(InputAction.CallbackContext context)
    {
        _useAction = context.action.triggered;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_interactWithTool && collision.CompareTag("Tool") && !_toolInHand)
        {
            PickupTool();
        }

    }


    private void PickupTool()
    {
        Actions.OnToolPickup(gameObject);

        _toolInHand = true;
    }

    private void DropTool()
    {
        Actions.OnToolDrop(gameObject);

        _toolInHand = false;
    }

}
