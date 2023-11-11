namespace CodeBase.InfraStructure
{
    public interface IStateFactory
    {
        IState Create<T>() where T : class, IState;
    }
}