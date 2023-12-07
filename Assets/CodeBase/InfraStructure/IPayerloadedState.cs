namespace CodeBase.InfraStructure
{
    public interface IPayloadedState<TPayload> : IExit
    {
        void Enter(TPayload payload);
    }
}