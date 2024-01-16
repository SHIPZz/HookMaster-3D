using System;
using CodeBase.Enums;
using CodeBase.Services.Providers.Camera;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace CodeBase.Services.CanvasCameraSet
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        [SerializeField] private float _planeDistance = 100f;
        [SerializeField] private SortingLayerTypeId _sortingLayerTypeId = SortingLayerTypeId.Default;
        
        private Canvas _canvas;
        private CameraProvider _cameraProvider;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _canvas = GetComponent<Canvas>();
            _cameraProvider = cameraProvider;
        }

        private void Start()
        {
            _canvas.worldCamera = _cameraProvider.Camera;
            _canvas.sortingLayerName = Enum.GetName(typeof(SortingLayerTypeId), _sortingLayerTypeId);
            _canvas.planeDistance = _planeDistance;
        }
    }
}