using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.UpgradeEmployee;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class UpgradeEmployeeUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _upPositionY = 2f;
        [SerializeField] private float _downPositionY = 5f;
        [SerializeField] private float _upPositionDuration = 0.5f;
        [SerializeField] private float _downPositionDuration = 0.2f;
        [SerializeField] private Employee _employee;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private RectTransformAnimator _rectTransformAnimator;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private Tween _movePositionTween;
        private Vector2 _initialAnchoredPosition;
        private CameraProvider _cameraProvider;
        private WindowService _windowService;

        [Inject]
        private void Construct(CameraProvider cameraProvider, WindowService windowService)
        {
            _windowService = windowService;
            _cameraProvider = cameraProvider;
        }

        private void Awake()
        {
            _upgradeButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnUpgradeButtonClicked()
        {
           var upgradeWindow =  _windowService.OpenAndGet<UpgradeEmployeeWindow>();
           upgradeWindow.Init(_employee);
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_employee.IsWorking)
                return;

            _upgradeButton.gameObject.SetActive(false);
            _canvasAnimator.FadeOutCanvas();
            _rectTransformAnimator.MoveAnchoredPositionY(-_downPositionY, _downPositionDuration,
                () => _upgradeButton.gameObject.SetActive(false));
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_employee.IsWorking)
                return;

            _upgradeButton.gameObject.SetActive(true);
            _rectTransformAnimator.SetInitialPosition();
            _canvasAnimator.FadeInCanvas();
            _rectTransformAnimator.MoveAnchoredPositionY(_upPositionY, _upPositionDuration);
            _rectTransformAnimator.SetRotation(Quaternion.LookRotation(_cameraProvider.Camera.transform.forward));
        }
    }
}