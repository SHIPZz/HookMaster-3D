using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeWorkUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Employee _employee;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private EmployeeWorkWindow _employeeWorkWindow;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _buttonRectTransform;
        [SerializeField] private float _downPositionY = 2f;
        [SerializeField] private float _appearDuration = 0.1f;
        [SerializeField] private float _downDuration = 0.5f;
        [SerializeField] private float _upDuration = 0.25f;

        private WindowService _windowService;
        private Tween _tween;
        private Tween _moveTween;
        private CameraProvider _cameraProvider;
        private Vector2 _initialButtonAnchoredPosition;

        [Inject]
        private void Construct(WindowService windowService, CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _windowService = windowService;
        }

        private void Awake()
        {
            _initialButtonAnchoredPosition = _buttonRectTransform.anchoredPosition;
            _buttonRectTransform.DOAnchorPosY(_initialButtonAnchoredPosition.y - _downPositionY, 0f);
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnPlayerExited(Collider obj)
        {
            if (_employee.IsWorking)
                return;

            _tween?.Kill(true);
            _tween = _canvasGroup.DOFade(0, _appearDuration).OnComplete(() => _canvas.enabled = false);
            _moveTween?.Kill(true);
            _moveTween =
                _buttonRectTransform.DOAnchorPosY(_initialButtonAnchoredPosition.y - _downPositionY, _downDuration)
                    .OnComplete(() => _employeeWorkWindow.InvokeEmployeeWorkButton.gameObject.SetActive(false));
            
            _windowService.Close<EmployeeWorkWindow>();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (_employee.IsWorking)
                return;

            _canvas.enabled = true;
            _employeeWorkWindow.InvokeEmployeeWorkButton.gameObject.SetActive(true);
            _tween?.Kill(true);
            _tween = _canvasGroup.DOFade(1, _appearDuration);
            _moveTween?.Kill(true);
            _moveTween = _buttonRectTransform.DOAnchorPosY(_initialButtonAnchoredPosition.y, _upDuration);
            _employeeWorkWindow.InvokeEmployeeWorkButton.transform.rotation =
                Quaternion.LookRotation(_cameraProvider.Camera.transform.forward,
                    _cameraProvider.Camera.transform.up);

            _employeeWorkWindow.SetLastTargetEmployee(_employee);
        }
    }
}