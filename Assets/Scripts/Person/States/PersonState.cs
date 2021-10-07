namespace PersonAndGhost.Person.States
{
    public class PersonState : State
    {
        protected PersonMovement character;

        public PersonState(PersonMovement character, StateMachine stateMachine) 
            : base(stateMachine)
        {
            this.character = character;
            this.stateMachine = stateMachine;
        }
    }
}