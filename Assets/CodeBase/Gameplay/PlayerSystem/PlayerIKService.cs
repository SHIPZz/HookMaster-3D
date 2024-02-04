using Cysharp.Threading.Tasks;
using DG.Tweening;
using RootMotion.FinalIK;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerIKService
    {
        private const float ClearSpeed = 17f;
        public bool HasItemInHands { get; private set; }
        
        private FullBodyBipedIK _fullBodyBipedIK;

        
        public void Set(FullBodyBipedIK fullBodyBipedIK) =>
            _fullBodyBipedIK = fullBodyBipedIK;
        
        public void SetIKHandTargets(Transform leftHandTarget, Transform rightHandTarget)
        {
            HasItemInHands = true;
            _fullBodyBipedIK.solver.leftHandEffector.target = leftHandTarget;
            _fullBodyBipedIK.solver.rightHandEffector.target = rightHandTarget;
            SetIKHandWeights(1);
        }

        public async void ClearIKHandTargets()
        {
            while (_fullBodyBipedIK.solver.leftHandEffector.positionWeight > 0.1f)
            {
                _fullBodyBipedIK.solver.leftHandEffector.positionWeight =
                    Mathf.Lerp(_fullBodyBipedIK.solver.leftHandEffector.positionWeight, 0, ClearSpeed * Time.deltaTime);
                
                _fullBodyBipedIK.solver.leftHandEffector.rotationWeight =
                    Mathf.Lerp(_fullBodyBipedIK.solver.leftHandEffector.rotationWeight, 0, ClearSpeed * Time.deltaTime);
                
                _fullBodyBipedIK.solver.rightHandEffector.positionWeight =
                    Mathf.Lerp(_fullBodyBipedIK.solver.leftHandEffector.positionWeight, 0, ClearSpeed * Time.deltaTime);
                
                _fullBodyBipedIK.solver.rightHandEffector.rotationWeight =
                    Mathf.Lerp(_fullBodyBipedIK.solver.leftHandEffector.rotationWeight, 0, ClearSpeed * Time.deltaTime);
                
                await UniTask.Yield();
            }
            
            // _fullBodyBipedIK.solver.leftHandEffector.target = null;
            // _fullBodyBipedIK.solver.rightHandEffector.target = null;

            SetIKHandWeights(0);
            HasItemInHands = false;
        }

        private void SetIKHandWeights(float weight)
        {
            _fullBodyBipedIK.solver.leftHandEffector.positionWeight = weight;
            _fullBodyBipedIK.solver.rightHandEffector.positionWeight = weight;
            _fullBodyBipedIK.solver.rightHandEffector.rotationWeight = weight;
            _fullBodyBipedIK.solver.leftHandEffector.rotationWeight = weight;
        }
    }
}