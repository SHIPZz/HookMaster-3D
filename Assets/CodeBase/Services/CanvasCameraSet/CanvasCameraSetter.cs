using System;
using CodeBase.Services.Providers.Camera;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CanvasCameraSet
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        private Canvas _canvas;
        private CameraProvider _cameraProvider;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _canvas = GetComponent<Canvas>();
            _cameraProvider = cameraProvider;
            _canvas.worldCamera = _cameraProvider.Camera;
        }

        // private void Awake() => 

        private void OnEnable()
        {
            _canvas.worldCamera = _cameraProvider.Camera;
        }
    }
}