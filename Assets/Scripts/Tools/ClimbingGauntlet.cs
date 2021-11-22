using UnityEngine;
using PersonAndGhost.Person;
using PersonAndGhost.Person.States;

namespace PersonAndGhost.Tools
{
    public class ClimbingGauntlet : Tool
    {
        private PersonMovement _obtainerController = default;
        private StateMachine _obtainerStateMachine = default;

        protected override void ToolAction(GameObject obtainer)
        {
            base.ToolAction(obtainer);

            if (_obtainerController != null && _obtainerStateMachine != null
                && _obtainerController.IsTouchingWall)
            {
                _obtainerStateMachine.ChangeState(_obtainerController.cling);
            }
        }

        protected override void ToolPickup(GameObject obtainer, GameObject tool)
        {
            base.ToolPickup(obtainer, tool);

            _obtainerController = obtainer.GetComponent<PersonMovement>();
            _obtainerStateMachine ??= _obtainerController.MovementSM;

            //Debug.Log("Climbing Gauntlet got picked up by: " + obtainer.name);
        }

        protected override void ToolDrop(GameObject obtainer, GameObject tool)
        {
            base.ToolDrop(obtainer, tool);

            _obtainerController = null;
            _obtainerStateMachine = null;

            //Debug.Log("Climbing Gauntlet got dropped by: " + obtainer.name);
        }
    }
}