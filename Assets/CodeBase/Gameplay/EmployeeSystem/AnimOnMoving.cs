using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class AnimOnMoving : ITickable
    {
        private const float MinimalVelocity = 0.1f;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EmployeeAnimator _employeeAnimator;

        public AnimOnMoving(EmployeeAnimator employeeAnimator, NavMeshAgent navMeshAgent)
        {
            _employeeAnimator = employeeAnimator;
            _navMeshAgent = navMeshAgent;
        }
        
        public void Tick()
        {
            if (_navMeshAgent.velocity.magnitude > MinimalVelocity)
            {
                _employeeAnimator.SetSpeed(1f);
                return;
            }

            _employeeAnimator.SetSpeed(0f);
        }
    }
}