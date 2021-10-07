using UnityEngine;
using UnityEngine.InputSystem;
using PersonAndGhost.Person;

public class PersonMeditation : MonoBehaviour
{
    [Header("Component Fields")]
    PersonMovement _movementScript = default;

    private void Start()
    {
        _movementScript = GetComponent<PersonMovement>();
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
