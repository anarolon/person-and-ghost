namespace PersonAndGhost.Person.States
{
    public class JumpingState : PersonState
    {
        public JumpingState(PersonMovement character, StateMachine stateMachine) 
            : base(character, stateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            character.Jump();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // TODO: maybe only change when at the height of the jump 
            //       (when y velocity == 0)
            stateMachine.ChangeState(character.falling);
        }

        public override string StateId() {
            return "JumpingState";
        }
    }
}