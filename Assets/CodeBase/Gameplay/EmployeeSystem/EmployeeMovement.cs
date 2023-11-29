using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Vector3 _targetPosition;

        [Inject]
        private void Construct(NavMeshAgent navMeshAgent) => 
            _navMeshAgent = navMeshAgent;

        private void Update()
        {
            if (_targetPosition != Vector3.zero)
                _navMeshAgent.SetDestination(_targetPosition);
        }

        public void SetTarget(Vector3 targetPosition) => 
            _targetPosition = targetPosition;
    }
}