using PersonAndGhost.Person;
using PersonAndGhost.Person.States;
using UnityEngine;

namespace PersonAndGhost.Tools
{
    public class GrapplingHook : Tool
    {
        private PersonMovement _obtainerController = default;
        private StateMachine _obtainerStateMachine = default;

        protected override void ToolAction(GameObject obtainer)
        {
            base.ToolAction(obtainer);

            if (_obtainerController != null && _obtainerStateMachine != null)
            {
                // TODO: Do Grappling Hook Stuff
                Debug.Log("Grapple!");
            }
        }

        protected override void ToolPickup(GameObject obtainer)
        {
            base.ToolPickup(obtainer);

            _obtainerController = obtainer.GetComponent<PersonMovement>();
            _obtainerStateMachine ??= _obtainerController.MovementSM;

            Debug.Log("Grappling Hook got picked up by: " + obtainer.name);
        }

        protected override void ToolDrop(GameObject obtainer)
        {
            base.ToolDrop(obtainer);

            _obtainerController = null;
            _obtainerStateMachine = null;

            Debug.Log("Grappling Hook got dropped by: " + obtainer.name);
        }
    }
}