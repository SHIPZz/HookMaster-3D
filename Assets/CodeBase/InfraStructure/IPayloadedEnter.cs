namespace CodeBase.InfraStructure
{
    public interface IPayloadedEnter<in T>
    {
        void Enter(T payload);
    }
}