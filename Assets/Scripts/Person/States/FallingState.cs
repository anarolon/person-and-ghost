using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class FallingState : PersonState
    {
        private Vector2 _movementInput;
        private bool _meditating;

        public FallingState(PersonMovement character, StateMachine stateMachine) 
            : base(character, stateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            _movementInput = character.MovementInput;

            _meditating = character.IsMeditating;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.IsOnGround)
            {
                stateMachine.ChangeState(character.idle);
            }

            else if (_meditating)
            {
                stateMachine.ChangeState(character.meditate);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.AirMove();
        }
    }
}