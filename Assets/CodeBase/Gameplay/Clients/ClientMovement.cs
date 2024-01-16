using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientMovement : IInitializable, ITickable
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly ClientAnimator _clientAnimator;
        private Transform _target;
        private bool _blocked;
        private Transform _clientTransform;
        private Vector3 _initialPosition;
        private bool _isMovingBack;

        public ClientMovement(NavMeshAgent navMeshAgent, ClientAnimator clientAnimator, Transform clientTransform)
        {
            _clientTransform = clientTransform;
            _clientAnimator = clientAnimator;
            _navMeshAgent = navMeshAgent;
        }

        public void Initialize()
        {
            _initialPosition = _clientTransform.position;
        }

        public void Tick()
        {
            if (_target == null || _blocked || _isMovingBack)
                return;
            
            _navMeshAgent.SetDestination(_target.position);
        }

        public void SetSitIdle(bool isSitIdle)
        {
            if (isSitIdle)
            {
                _blocked = true;
                _navMeshAgent.enabled = false;
                _clientTransform.position = _target.position;
                _clientTransform.rotation = Quaternion.LookRotation(_target.transform.forward);
                _clientAnimator.SetIsSitIdle(true);
                return;
            }

            _blocked = false;
            _navMeshAgent.enabled = true;
            _clientAnimator.SetIsSitIdle(false);
        }

        public async void MoveBack(Action onComplete = null)
        {
            _isMovingBack = true;
            SetTarget(null);

            while (_clientTransform != null && Vector3.Distance(_clientTransform.position, _initialPosition) > 0.5f)
            {
                _navMeshAgent.SetDestination(_initialPosition);
                await UniTask.Yield();
            }

            _isMovingBack = false;
            onComplete?.Invoke();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public bool IsMovingBack() => 
            _isMovingBack;
    }
}