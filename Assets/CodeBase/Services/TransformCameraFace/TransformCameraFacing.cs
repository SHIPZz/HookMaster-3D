using System;
using CodeBase.Services.Providers.Camera;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CameraRotation
{
    public class TransformCameraFacing : MonoBehaviour
    {
        private CameraProvider _cameraProvider;

        [Inject]
        private void Construct(CameraProvider cameraProvider) =>
            _cameraProvider = cameraProvider;

        private void Update()
        {
            if (_cameraProvider.Camera == null)
                return;
            
            transform.rotation = Quaternion.LookRotation(_cameraProvider.Camera.transform.forward);
        }

        private async void OnEnable()
        {
            while (_cameraProvider.Camera == null || _cameraProvider.Rotating)
            {
                await UniTask.Yield();
            }

            transform.rotation = Quaternion.LookRotation(_cameraProvider.Camera.transform.forward);
        }
    }
}