using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientMovement : ITickable
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly ClientAnimator _clientAnimator;
        private Vector3 _target;
        private bool _blocked;

        public ClientMovement(NavMeshAgent navMeshAgent, ClientAnimator clientAnimator)
        {
            _clientAnimator = clientAnimator;
            _navMeshAgent = navMeshAgent;
        }

        public void Tick()
        {
            if (_target == Vector3.zero || _blocked)
                return;

            if (_navMeshAgent.remainingDistance >= 0.1f)
            {
                _navMeshAgent.SetDestination(_target);
            }
        }

        public void SetSitIdle(bool isSitIdle, Transform target, Client client)
        {
            if (isSitIdle)
            {
                _blocked = true;
                _navMeshAgent.enabled = false;
                client.transform.position = target.position;
                client.transform.rotation = Quaternion.LookRotation(target.transform.forward);
            }
            
            _clientAnimator.SetIsSitIdle(isSitIdle);
        }

        public void SetTarget(Vector3 target)
        {
            _target = target;
        }
    }
}