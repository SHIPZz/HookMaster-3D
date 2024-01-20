using CodeBase.Services.Providers.Location;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Clients
{
    public class ClientMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _sitDistance = 0.6f;
        [SerializeField] private float _rotateDistance = 0.5f;
        [SerializeField] private float _rotationSpeed = 5f;
        
        private ClientAnimator _clientAnimator;
        private LocationProvider _locationProvider;
        private Transform _target;
        private bool _blocked;
        private bool _needSitIdle;

        [Inject]
        public void Construct(ClientAnimator clientAnimator, LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _clientAnimator = clientAnimator;
        }

        public void Update()
        {
            if (_target == null || _blocked)
                return;

            _navMeshAgent.SetDestination(_target.position);

            if (_navMeshAgent.remainingDistance <= _rotateDistance)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(_target.forward), _rotationSpeed * Time.deltaTime);
        }

        public void SetSitIdle(bool isSitIdle)
        {
            if (isSitIdle)
            {
                _blocked = true;
                _navMeshAgent.enabled = false;
                transform.position = _target.position;
                transform.rotation = Quaternion.LookRotation(_target.transform.forward);
                _needSitIdle = false;
                _clientAnimator.SetIsSitIdle(true);
                return;
            }

            _blocked = false;
            _navMeshAgent.enabled = true;
            _clientAnimator.SetIsSitIdle(false);
        }

        public async void SetSitIdleByMoving(Transform target)
        {
            _blocked = true;
            
            while (Vector3.Distance(transform.position, target.position) >= _sitDistance)
            {
                _navMeshAgent.SetDestination(target.position);

                await UniTask.Yield();
            }
            
            _target = target;
            SetSitIdle(true);
        }

        public void MoveBack()
        {
            _target = _locationProvider.DisableClientZone;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}