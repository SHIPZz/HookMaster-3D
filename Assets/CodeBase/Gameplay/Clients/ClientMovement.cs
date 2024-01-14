using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientMovement : ITickable
    {
        private readonly NavMeshAgent _navMeshAgent;
        private Vector3 _target;

        public ClientMovement(NavMeshAgent navMeshAgent)
        {
            _navMeshAgent = navMeshAgent;
        }

        public void Tick()
        {
            if (_target == Vector3.zero)
                return;

            if (_navMeshAgent.remainingDistance >= 0.1f)
            {
                _navMeshAgent.SetDestination(_target);
            }
        }

        public void BlockMovement(bool isBlocked) { }

        public void SetTarget(Vector3 target)
        {
            _target = target;
        }
    }
}