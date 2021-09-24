using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolController : MonoBehaviour
{
    [SerializeField] private Tool tool;
    [SerializeField] private bool toolInHand = false;

    [Header("Input System Variables")]
    [SerializeField] private bool interactWithTool = false;
    private bool useAction = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (useAction && toolInHand)
        {
            tool.Action();
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactWithTool)
        {
            InteractWithTool(other.gameObject.GetComponent<Tool>());
            interactWithTool = false;
        }
    }

    //TESTING
    public void InteractWithTool(Tool newTool)
    {
        if (newTool == null) return;

        if (toolInHand)
        {
            tool.GetDropped();
            tool = null;
        }
        else
        {
            tool = newTool;
            tool.GetPickedUp(GetComponent<PlayerController>());
        }
        toolInHand = !toolInHand;
    }
}
