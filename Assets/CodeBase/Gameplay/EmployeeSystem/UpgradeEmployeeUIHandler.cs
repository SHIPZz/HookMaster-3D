using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.UI.UpgradeEmployee;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class UpgradeEmployeeUIHandler : MonoBehaviour
    {
        [SerializeField] private UpgradeEmployeeWindow _upgradeEmployeeWindow;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _canvasFadeDuration = 0.5f;
        [SerializeField] private float _canvasNotFadeDuration = 0.2f;
        [SerializeField] private RectTransform _upgradeWindowRectTransform;
        [SerializeField] private float _upPositionY = 2f;
        [SerializeField] private float _downPositionY = 5f;
        [SerializeField] private float _upPositionDuration = 0.5f;
        [SerializeField] private float _downPositionDuration = 0.2f;
        [SerializeField] private Employee _employee;

        private Tween _canvasFadeTween;
        private Tween _movePositionTween;
        private Vector2 _initialAnchoredPosition;
        private CameraProvider _cameraProvider;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        private void Awake()
        {
            _initialAnchoredPosition = _upgradeWindowRectTransform.anchoredPosition;
            _upgradeEmployeeWindow.gameObject.SetActive(false);
            _canvasGroup.interactable = false;
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
            if (!_employee.IsWorking)
                return;

            _canvasFadeTween?.Kill(true);
            _canvasFadeTween = _canvasGroup.DOFade(0f, _canvasNotFadeDuration);
            _movePositionTween?.Kill(true);

            _movePositionTween =
                _upgradeWindowRectTransform.DOAnchorPosY(
                        _upgradeWindowRectTransform.anchoredPosition.y - _downPositionY,
                        _downPositionDuration)
                    .OnComplete(_upgradeEmployeeWindow.Close);
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_employee.IsWorking)
                return;
            
            _upgradeEmployeeWindow.Open();
            _canvasGroup.interactable = true;
            _upgradeWindowRectTransform.anchoredPosition = _initialAnchoredPosition;
            _canvasFadeTween?.Kill(true);
            _canvasFadeTween = _canvasGroup.DOFade(1f, _canvasFadeDuration);
            _movePositionTween?.Kill(true);
            _upgradeWindowRectTransform.rotation = Quaternion.LookRotation(_cameraProvider.Camera.transform.forward);
            _movePositionTween =
                _upgradeWindowRectTransform.DOAnchorPosY(_initialAnchoredPosition.y + _upPositionY,
                    _upPositionDuration);
        }
    }
}