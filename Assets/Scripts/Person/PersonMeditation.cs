using UnityEngine;
using UnityEngine.InputSystem;

public class PersonMeditation : MonoBehaviour
{
    [Header("Component Fields")]
    PlayerController _movementScript = default;

    private void Start()
    {
        _movementScript = GetComponent<PlayerController>();
    }

    public void OnMeditation(InputAction.CallbackContext context)
    {
        context.ReadValue<float>();

        bool triggered = context.action.triggered;

        if (triggered)
        {
            _movementScript.IsMeditating = !_movementScript.IsMeditating;
        }
    }
}
