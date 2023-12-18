using CodeBase.Gameplay.TableSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeMovement : MonoBehaviour
    {
        [SerializeField] private float _remainingDistance = 0.1f;
        [SerializeField] private Employee _employee;
        
        private NavMeshAgent _navMeshAgent;
        private Table _targetTable;
        private EmployeeAnimator _employeeAnimator;
        
        public bool IsMovingToTable { get; private set; }

        [Inject]
        private void Construct(NavMeshAgent navMeshAgent, EmployeeAnimator employeeAnimator)
        {
            _employeeAnimator = employeeAnimator;
            _navMeshAgent = navMeshAgent;
        }

        private void Update()
        {
            if (_targetTable == null || !_navMeshAgent.isActiveAndEnabled)
                return;

            _navMeshAgent.SetDestination(_targetTable.transform.position);
            IsMovingToTable = true;
            
            if (_navMeshAgent.remainingDistance == 0 || !(_navMeshAgent.remainingDistance < _remainingDistance)) 
                return;

            Sit();
        }

        public void Sit()
        {
            _employeeAnimator.SetSitTyping(true);
            _navMeshAgent.enabled = false;
            transform.rotation = Quaternion.LookRotation(_targetTable.transform.forward);
            transform.position = _targetTable.Chair.position;
            _employee.StartWorking();
        }

        public void SetTable(Table target) =>
            _targetTable = target;
    }
}