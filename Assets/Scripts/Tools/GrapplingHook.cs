using PersonAndGhost.Person;
using PersonAndGhost.Person.States;
using UnityEngine;

namespace PersonAndGhost.Tools
{
    // TDOO: Require Spring joint 2d component
    public class GrapplingHook : Tool
    {
        private PersonMovement _obtainerController = default;
        private StateMachine _obtainerStateMachine = default;

        [SerializeField] private LayerMask _wallLayer;


        protected override void ToolAction(GameObject obtainer)
        {
            base.ToolAction(obtainer);

            if (_obtainerController != null && _obtainerStateMachine != null)
            {
                _obtainerStateMachine.ChangeState(_obtainerController.grappleAim);
            }
        }

        protected override void ToolPickup(GameObject obtainer, GameObject tool)
        {
            base.ToolPickup(obtainer, tool);

            _obtainerController = obtainer.GetComponent<PersonMovement>();
            _obtainerStateMachine ??= _obtainerController.MovementSM;

            Debug.Log("Grappling Hook got picked up by: " + obtainer.name);
        }

        protected override void ToolDrop(GameObject obtainer, GameObject tool)
        {
            base.ToolDrop(obtainer, tool);

            _obtainerController = null;
            _obtainerStateMachine = null;

            Debug.Log("Grappling Hook got dropped by: " + obtainer.name);
        }
    }
}