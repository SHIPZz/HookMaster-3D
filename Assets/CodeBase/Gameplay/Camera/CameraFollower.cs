using UnityEngine;

namespace CodeBase.Gameplay.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed = 3f;
        private Transform _target;

        private void LateUpdate()
        {
            if(_target == null)
                return;
        
            transform.position = _target.position + _offset * _speed;
        }

        public void SetTarget(Transform playerTransform)
        {
            _target = playerTransform;
        }
    }
}