﻿using System;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Camera
{
    public class CameraRotationTrigger : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Vector3 _targetRotation;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private bool _needReturnOnExit;

        private Tween _tween;
        private CameraProvider _cameraProvider;
        private Vector3 _initialRotation;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        private void Start()
        {
            _initialRotation = _cameraProvider.Camera.transform.eulerAngles;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += RotateOnTrigger;
            _triggerObserver.TriggerExited += OnRotateBack;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= RotateOnTrigger;
            _triggerObserver.TriggerExited -= OnRotateBack;
        }

        private void OnRotateBack(Collider obj)
        {
            if(!_needReturnOnExit)
                return;

            _tween?.Kill();

            _cameraProvider.Rotating = true;
            _tween = _cameraProvider.Camera.transform.DORotate(_initialRotation, _duration).OnComplete(() => _cameraProvider.Rotating = false);

        }

        private void RotateOnTrigger(Collider obj)
        {
            _tween?.Kill(true);

            float x = Mathf.Approximately(_targetRotation.x, 0f) ? _cameraProvider.Camera.transform.eulerAngles.x : _targetRotation.x;
            float y = Mathf.Approximately(_targetRotation.y, 0f) ? _cameraProvider.Camera.transform.eulerAngles.y : _targetRotation.y;
            float z = Mathf.Approximately(_targetRotation.z, 0f) ? _cameraProvider.Camera.transform.eulerAngles.z : _targetRotation.z;

            Vector3 newTargetRotation = new Vector3(x, y, z);
            _cameraProvider.Rotating = true;
            _tween = _cameraProvider.Camera.transform.DORotate(newTargetRotation, _duration).OnComplete(() => _cameraProvider.Rotating = false);

            if (_targetPosition != Vector3.zero)
            {
                _cameraProvider.CameraFollower.SetTargetOffset(_targetPosition);
            }
        }
    }
}