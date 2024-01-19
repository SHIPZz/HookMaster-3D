using System;
using System.Collections.Generic;

namespace CodeBase.InfraStructure
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states = new();
        private readonly IStateFactory _stateFactory;
        private IState _currentState;

        public GameStateMachine(IStateFactory stateFactory) =>
            _stateFactory = stateFactory;

        public void ChangeState<T>() where T : class, IState
        {
            (_currentState as IExit)?.Exit();

            IState state = _stateFactory.Create<T>();

            (state as IEnter)?.Enter();
            _currentState = state;
        }

        public void ChangeState<TState, TPayload>(TPayload payload)
            where TState : class, IState, IPayloadedEnter<TPayload>
        {
            (_currentState as IExit)?.Exit();

            IState state = _stateFactory.Create<TState>();

            (state as IPayloadedEnter<TPayload>)?.Enter(payload);
            _currentState = state;
        }
    }
}