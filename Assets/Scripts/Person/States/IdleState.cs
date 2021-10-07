using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class IdleState : GroundedPersonState
    {
        private Vector2 _movementInput;
        private bool _jumped;
        private bool _meditating;

        public IdleState(PersonMovement character, StateMachine stateMachine) 
            : base(character, stateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            character.ResetVelocity();
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

            if (_movementInput.x != 0)
            {
                stateMachine.ChangeState(character.movement);
            }

            else if (_jumped)
            {
                stateMachine.ChangeState(character.jumping);
            }

            else if (_meditating)
            {
                stateMachine.ChangeState(character.meditate);
            }
        }
    }
}