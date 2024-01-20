using System;
using CodeBase.Services.Providers.Location;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientMovement : ITickable
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly ClientAnimator _clientAnimator;
        private readonly LocationProvider _locationProvider;
        private readonly Transform _clientTransform;
        private Transform _target;
        private bool _blocked;
        private bool _isMovingBack;

        public ClientMovement(NavMeshAgent navMeshAgent, ClientAnimator clientAnimator, Transform clientTransform,
            LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _clientTransform = clientTransform;
            _clientAnimator = clientAnimator;
            _navMeshAgent = navMeshAgent;
        }

        public void Tick()
        {
            if (_target == null || _blocked || _isMovingBack)
                return;

            _navMeshAgent.SetDestination(_target.position);

            if (_navMeshAgent.remainingDistance <= 0.5f)
                _clientTransform.rotation = Quaternion.Slerp(_clientTransform.rotation,
                    Quaternion.LookRotation(_target.forward), 5 * Time.deltaTime);
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

        public async void SetSitIdleByMoving()
        {
            _blocked = true;
            Debug.Log(_navMeshAgent);

            while (_clientTransform != null && Vector3.Distance(_clientTransform.position, _target.position) > 0.2f)
            {
                _navMeshAgent.SetDestination(_target.position);
                Debug.Log(_navMeshAgent.remainingDistance);

                await UniTask.Yield();
            }

            SetSitIdle(true);
        }

        public async void MoveBack(Action onComplete = null)
        {
            _isMovingBack = true;
            SetTarget(null);

            while (_clientTransform != null &&
                   Vector3.Distance(_clientTransform.position, _locationProvider.DisableClientZone.position) > 0.1f)
            {
                if (!_navMeshAgent.gameObject.activeSelf)
                {
                    onComplete?.Invoke();
                    break;
                }
                
                _navMeshAgent.SetDestination(_locationProvider.DisableClientZone.position);

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