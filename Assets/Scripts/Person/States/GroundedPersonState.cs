namespace PersonAndGhost.Person.States
{
    public class GroundedPersonState : PersonState
    {

        public GroundedPersonState(PersonMovement character, StateMachine stateMachine) 
            : base(character, stateMachine)
        {

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!character.IsOnGround)
            {
                stateMachine.ChangeState(character.falling);
            }
        }
    }
}