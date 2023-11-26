using System;
using System.Collections;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.BulletSystem
{
    public class BulletMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private TriggerObserver _triggerObserver;
        

        private Coroutine _moveCoroutine;
        private float _speed;
        private bool _isTriggered;

        private void OnEnable()
        {
            _triggerObserver.CollisionEntered += collision => _isTriggered = true;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        public void Move(Vector3 moveDirection, Vector3 startPosition)
        {
            transform.position = startPosition;

            if(_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            
            _moveCoroutine = StartCoroutine(MoveCoroutine(moveDirection,startPosition.normalized));
        }

        private void OnCollisionEnter(Collision other)
        {
            _isTriggered = true;
            print(_isTriggered);
        }

        private void OnTriggerEnter(Collider other)
        {
        }

        private IEnumerator MoveCoroutine(Vector3 direction, Vector3 startPosition)
        {
            while (!_isTriggered)
            {
                if(_isTriggered)
                    break;
                
                Vector3 targetScale = transform.localScale + new Vector3(0, 0.1f, 0f);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, _speed * Time.deltaTime);
                transform.up = direction;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}