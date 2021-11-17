using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class ClingState : PersonState
    {
        private Vector2 _movementInput;
        private bool _jumped;
        private bool _meditating;

        public ClingState(PersonMovement character, StateMachine stateMachine) 
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

            // TODO: Make the player fall if they move towards the wall they are clinging
            if (_meditating)
            {
                stateMachine.ChangeState(character.meditate);
            }

            else if (_movementInput.x != 0 || _jumped)
            {
                stateMachine.ChangeState(character.jumping);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.Cling();
        }

        public override void Exit()
        {
            base.Exit();
            character.UnCling();
        }

         public override string StateId() {
            return "ClingState";
        }
    }
}