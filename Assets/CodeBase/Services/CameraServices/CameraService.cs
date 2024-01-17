using CodeBase.Services.Providers.Camera;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Services.Camera
{
    public class CameraService
    {
        private readonly CameraProvider _cameraProvider;
        private Transform _target;

        public CameraService(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void MoveTo()
        {
            _cameraProvider.CameraFollower.Block(true);
            _cameraProvider.CameraFollower.MoveTo(_target);
            // _cameraProvider.CameraFollower.transform.DOMove(_target + _cameraProvider.CameraFollower.CameraPanOffset, duration);
        }
    }
}