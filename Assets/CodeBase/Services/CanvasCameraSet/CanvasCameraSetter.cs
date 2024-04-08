using System;
using CodeBase.Enums;
using CodeBase.Services.CameraServices;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CanvasCameraSet
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        [SerializeField] private float _planeDistance = 100f;
        [SerializeField] private SortingLayerTypeId _sortingLayerTypeId = SortingLayerTypeId.Default;
        
        private Canvas _canvas;
        private CameraController _cameraController;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _canvas = GetComponent<Canvas>();
            _cameraController = cameraController;
        }

        private void Start()
        {
            _canvas.worldCamera = _cameraController.Camera.GetComponent<Camera>();
            _canvas.sortingLayerName = Enum.GetName(typeof(SortingLayerTypeId), _sortingLayerTypeId);
            _canvas.planeDistance = _planeDistance;
        }
    }
}