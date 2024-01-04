using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CodeBase.Services.Reward;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Roulette;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

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
    [SerializeField] private TMP_Text _rotateCountText;
    [SerializeField] private List<RouletteItem> _rouletteItems;
    [SerializeField] private TriggerObserver _arrowTrigger;
    [SerializeField] private TMP_Text _allMoneyText;
    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private AudioSource _rotateSound;
    [SerializeField] private AudioSource _winSound;

    private Vector3 _lastRotation;

    private Coroutine _rotateCoroutine;
    private RouletteItem _lastDroppedRouletteItem;
    private RewardService _rewardService;
    private WindowService _windowService;
    private bool _rotated;

    [Inject]
    private void Construct(RewardService rewardService, WindowService windowService)
    {
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

    public override void Open()
    {
        _rouletteItems.ForEach(x => x.Init());
        _canvasAnimator.FadeInCanvas();
    }

    private void SetLastDroppedObject(Collider obj)
    {
        _lastDroppedRouletteItem = obj.GetComponent<RouletteItem>();

        if (!_rotated)
            return;

        _allMoneyText.text = $"{_lastDroppedRouletteItem.Quantity}$";
    }

    public override void Close() =>
        _canvasAnimator.FadeOutCanvas(base.Close);

    private void Rotate()
    {
        _rotated = true;
        _rotateCount = Mathf.Clamp(_rotateCount--, 0, _rotateCount);
        _rotateCountText.text = _rotateCount.ToString(CultureInfo.InvariantCulture);

        _rotateButton.interactable = false;

        if (_rotateCount == 0)
            _rotateButton.gameObject.SetActive(false);

        if (_rotateCoroutine != null)
            StopCoroutine(_rotateCoroutine);

        _rotateCoroutine = StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        yield return new WaitForSeconds(_startDelay);

        _rotateSound.Play();
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
        _effects.ForEach(x => x.Play());
        _winSound.Play();
        _rotateSound.Stop();

        if (_rotateCount == 0)
        {
            await UniTask.WaitForSeconds(_destroyDelay);
            _windowService.Open<RewardRouletteWindow>();
            Close();
        }
    }
}