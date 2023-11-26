using System;
using UnityEngine;
using com.ootii.Geometry;
using com.ootii.Utilities.Debug;

namespace com.ootii.Demos
{
    public class PathFollower : MonoBehaviour
    {
        public BezierSpline Path;

        public float Speed = 1f;

        public float SpeedMultiplier = 1f;

        public Quaternion RotateForward = Quaternion.identity;

        public float DistanceTraveled = 0f;

        private float mPathLength = 0f;

        private Vector3 mLastPosition = Vector3.zero;

        // Use this for initialization
        void Start()
        {
            mPathLength = Path.Length;
        }

        // Update is called once per frame
        void Update()
        {
            DistanceTraveled += Speed * SpeedMultiplier * Time.deltaTime;
            if (DistanceTraveled > mPathLength) { DistanceTraveled -= mPathLength; }

            float lPercent = DistanceTraveled / mPathLength;

            Vector3 lPosition = Path.GetPoint(lPercent);
            if (mLastPosition == Vector3.zero) { mLastPosition = lPosition; }

            gameObject.transform.position = lPosition;

            if ((lPosition - mLastPosition).sqrMagnitude != 0f)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(lPosition - mLastPosition, Vector3.up) * RotateForward;
            }

            mLastPosition = lPosition;
        }
    }
}
