using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientAnimOnMoving : ITickable
    {
        private const float MinimalVelocity = 0.1f;
        private readonly ClientAnimator _clientAnimator;
        private readonly NavMeshAgent _navMeshAgent;

        public ClientAnimOnMoving(ClientAnimator clientAnimator, NavMeshAgent navMeshAgent)
        {
            _clientAnimator = clientAnimator;
            _navMeshAgent = navMeshAgent;
        }

        public void Tick()
        {
            if (_navMeshAgent.velocity.sqrMagnitude > MinimalVelocity)
            {
                _clientAnimator.SetSpeed(1f);
                return;
            }

            _clientAnimator.SetSpeed(0f);
        }
    }
}