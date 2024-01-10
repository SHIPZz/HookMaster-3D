using CodeBase.Services.TriggerObserve;
using RootMotion.FinalIK;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class IKObjectTaker : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _ikTriggerObserver;
        [SerializeField] private FullBodyBipedIK _fullBodyBipedIK;
        private bool _isTaken;

        private void OnEnable()
        {
            _ikTriggerObserver.TriggerEntered += OnApproachedToIK;
            _ikTriggerObserver.TriggerExited += OnExited;
        }

        private void OnDisable()
        {
            _ikTriggerObserver.TriggerEntered -= OnApproachedToIK;
            _ikTriggerObserver.TriggerExited -= OnExited;
        }

        private void OnExited(Collider obj)
        {
            // ClearIKTargets();
        }

        public void Drop()
        {
            ClearIKTargets();
            _fullBodyBipedIK.solver.rightHandEffector.rotationWeight = 0;
            _fullBodyBipedIK.solver.leftHandEffector.rotationWeight = 0;
        }

        private void OnApproachedToIK(Collider obj)
        {
            if (_isTaken)
                return;

            var ikObject = obj.GetComponent<IKObjectSystem>();

            SetIKTargets(ikObject.LeftHandIK, ikObject.RightHandIK);
            _isTaken = true;
        }

        private void SetIKTargets(Transform leftHandTarget, Transform rightHandTarget)
        {
            _fullBodyBipedIK.solver.leftHandEffector.target = leftHandTarget;
            _fullBodyBipedIK.solver.rightHandEffector.target = rightHandTarget;
            SetIKWeights(1);
        }

        private void SetIKWeights(float weight)
        {
            _fullBodyBipedIK.solver.leftHandEffector.positionWeight = weight;
            _fullBodyBipedIK.solver.rightHandEffector.positionWeight = weight;
            _fullBodyBipedIK.solver.rightHandEffector.rotationWeight = weight;
            _fullBodyBipedIK.solver.leftHandEffector.rotationWeight = weight;
        }

        private void ClearIKTargets()
        {
            _fullBodyBipedIK.solver.leftHandEffector.target = null;
            _fullBodyBipedIK.solver.rightHandEffector.target = null;
            SetIKWeights(0);
        }
    }
}