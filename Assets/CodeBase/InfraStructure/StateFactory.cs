using Zenject;

namespace CodeBase.InfraStructure
{
    public class StateFactory : IStateFactory
    {
        private readonly IInstantiator _instantiator;

        public StateFactory(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public IState Create<T>() where T : class, IState
        {
            var state = _instantiator.Instantiate(typeof(T));

            return state as IState;
        }
    }
}