namespace PersonAndGhost.Person.States
{
    public class StateMachine
    {
        public State CurrentState { get; set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;

            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();

            CurrentState = newState;

            newState.Enter();
        }
    }
}
