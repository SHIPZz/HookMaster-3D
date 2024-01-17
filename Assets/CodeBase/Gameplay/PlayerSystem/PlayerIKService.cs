﻿using RootMotion.FinalIK;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerIKService
    {
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

        public void ClearIKHandTargets()
        {
            _fullBodyBipedIK.solver.leftHandEffector.target = null;
            _fullBodyBipedIK.solver.rightHandEffector.target = null;
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