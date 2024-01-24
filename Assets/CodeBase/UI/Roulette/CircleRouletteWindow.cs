using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CodeBase.Animations;
using CodeBase.Gameplay;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Reward;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.UI.Roulette
{
    public class CircleRouletteWindow : WindowBase
    {
        [SerializeField] private float _minRotationZ = 180f;
        [SerializeField] private float _maxRotationZ = 720f;
        [SerializeField] private float _rotateDuration = 1.5f;
        [SerializeField] private float _startDelay = 0.5f;
        [SerializeField] private float _destroyDelay = 0.5f;
        [SerializeField] private float _rotateCount = 5;
        [SerializeField] private Transform _target;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private Button _rotateButton;
        [SerializeField] private List<RouletteItem> _rouletteItems;
        [SerializeField] private TriggerObserver _arrowTrigger;
        [SerializeField] private TMP_Text _allMoneyText;
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private ImageFadeAnim _circleFadeAnim;
        [SerializeField] private TextAnimView _countTextAnim;

        private Vector3 _lastRotation;

        private Coroutine _rotateCoroutine;
        private RouletteItem _lastDroppedRouletteItem;
        private RewardService _rewardService;
        private WindowService _windowService;
        private bool _rotated;
        private CircleRouletteItem _circleRouletteItem;
        private CircleRouletteService _circleRouletteService;

        [Inject]
        private void Construct(RewardService rewardService, WindowService windowService,
            CircleRouletteService circleRouletteService)
        {
            _circleRouletteService = circleRouletteService;
            _windowService = windowService;
            _rewardService = rewardService;
        }

        private void OnEnable()
        {
            _rotateButton.onClick.AddListener(Rotate);
            _arrowTrigger.TriggerEntered += SetLastDroppedObject;
        }

        private void OnDisable()
        {
            _rotateButton.onClick.RemoveListener(Rotate);
            _arrowTrigger.TriggerEntered -= SetLastDroppedObject;
        }

        public void Init(CircleRouletteItem circleRouletteItem)
        {
            _circleRouletteItem = circleRouletteItem;
        }

        public override void Open()
        {
            _rouletteItems.ForEach(x => x.Init(_circleRouletteItem.MinWinValue, _circleRouletteItem.MaxWinValue));

            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            if (_rotated)
                _circleRouletteService.SetLastPlayedTime(_circleRouletteItem.Id);
            else
                _windowService.Open<HudWindow>();

            _canvasAnimator.FadeOutCanvas(base.Close);
        }

        private void SetLastDroppedObject(Collider obj)
        {
            _lastDroppedRouletteItem = obj.GetComponent<RouletteItem>();

            if (!_rotated)
                return;

            _allMoneyText.text = $"{_lastDroppedRouletteItem.Quantity}$";
        }

        private void Rotate()
        {
            CloseButton.transform.parent.gameObject.SetActive(false);
            _rotated = true;
            _rotateCount = Mathf.Clamp(_rotateCount--, 0, _rotateCount);
            AnimateUI();

            if (_rotateCount == 0)
                _rotateButton.gameObject.SetActive(false);

            if (_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);

            _rotateCoroutine = StartCoroutine(RotateCoroutine());
        }

        private IEnumerator RotateCoroutine()
        {
            yield return new WaitForSeconds(_startDelay);

            _soundPlayerSystem.PlayActiveSound();
            _lastRotation = _target.localEulerAngles;

            float randomRotationZ = Random.Range(_minRotationZ, _maxRotationZ);
            Vector3 targetRotation = _lastRotation + new Vector3(0, 0, randomRotationZ);

            float elapsedTime = 0f;

            while (elapsedTime < _rotateDuration)
            {
                float t = elapsedTime / _rotateDuration;
                t = Mathf.SmoothStep(0, 1, t);

                _target.localEulerAngles = Vector3.Lerp(_lastRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            CompleteRotation(targetRotation);
        }

        private async void CompleteRotation(Vector3 targetRotation)
        {
            _rewardService.AddRouletteReward(_lastDroppedRouletteItem);
            _target.localEulerAngles = targetRotation;

            _rotateButton.interactable = true;
            PlayEffects();

            if (_rotateCount == 0)
            {
                await UniTask.WaitForSeconds(_destroyDelay);
                _windowService.Open<RewardRouletteWindow>();
                Close();
            }
        }

        private void PlayEffects()
        {
            _appearanceEffect.PlayTargetEffects();
            _circleFadeAnim.FadeIn();
            _countTextAnim.DoFade();
            _soundPlayerSystem.PlayInactiveSound();
            _soundPlayerSystem.StopActiveSound();
        }

        private void AnimateUI()
        {
            _countTextAnim.SetText(_rotateCount.ToString(CultureInfo.InvariantCulture));
            _countTextAnim.DoFadeOut();
            _circleFadeAnim.FadeOut();

            _rotateButton.interactable = false;
        }
    }
}