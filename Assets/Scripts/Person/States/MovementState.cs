using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class MovementState : GroundedPersonState
    {
        private Vector2 _movementInput;
        private bool _jumped;
        private bool _meditating;

        public MovementState(PersonMovement character, StateMachine stateMachine) 
            : base(character, stateMachine)
        {

        }

        public override void HandleInput()
        {
            base.HandleInput();
            _movementInput = character.MovementInput;
            _jumped = character.Jumped;

            _meditating = character.IsMeditating;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_movementInput == Vector2.zero)
            {
                stateMachine.ChangeState(character.idle);
            }

            else if (_jumped)
            {
                //stateMachine.ChangeState(character.jumping);
                character.Jump();
            }

            else if (_meditating)
            {
                stateMachine.ChangeState(character.meditate);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.Move();
        }

        public override string StateId() {
            return "MovementState";
        }
    }
}