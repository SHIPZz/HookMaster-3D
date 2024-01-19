namespace CodeBase.InfraStructure
{
    public interface IGameStateMachine
    {
        void ChangeState<T>() where T : class, IState;
        void ChangeState<TState, TPayload>(TPayload payload) where TState : class, IState, IPayloadedEnter<TPayload>;
    }
}