using UnityEngine;

namespace PersonAndGhost.Person.States
{
    public class CarriedState : PersonState
    {
        private bool carried;

        public CarriedState(PersonMovement character, StateMachine stateMachine) 
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
            carried = character.IsBeingCarried;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!carried)
            {
                if (character.IsOnGround)
                {
                    stateMachine.ChangeState(character.idle);
                }

                else
                {
                    stateMachine.ChangeState(character.falling);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
