using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using CodeBase.Services.GOPool;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeProfitUIHandler : MonoBehaviour
    {
        [SerializeField] private Employee _employee;
        [SerializeField] private float _additionalAnchoredPositionY = 3f;
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _moveTextDuration = 1f;
        [SerializeField] private float _pushEffectPoolDelay = 2f;

        private ProfitService _profitService;
        private Vector2 _initialTextAnchoredPosition;
        private FloatingTextService _floatingTextService;
        private EffectPool _effectPool;

        [Inject]
        private void Construct(ProfitService profitService,
            [Inject(Id = ColorTypeId.Money)] Color moneyColor,
            FloatingTextService floatingTextService,
            EffectPool effectPool)
        {
            _effectPool = effectPool;
            _floatingTextService = floatingTextService;
            _profitService = profitService;
        }

        private void OnEnable() =>
            _profitService.ProfitGot += OnProfitGot;

        private void OnDisable() =>
            _profitService.ProfitGot -= OnProfitGot;

        [Button]
        private void OnProfitGot(string employeeId, float minuteProfit)
        {
            if (_employee.Id != employeeId)
                return;

            EffectView dollarBlowEffect = _effectPool.Pop(EffectTypeId.DollarBlow);

            dollarBlowEffect.transform.position = _employee.transform.position;
            dollarBlowEffect.Effect.Play();

            _floatingTextService
                .ShowFloatingText($"{minuteProfit}$",
                    _additionalAnchoredPositionY,
                    _moveTextDuration, _fadeInDuration, _fadeOutDuration,
                    Quaternion.identity,
                    AssetPath.ProfitText, _employee.transform);

            DOTween.Sequence()
                .AppendInterval(_pushEffectPoolDelay)
                .OnComplete(() => _effectPool.Push(dollarBlowEffect));
        }
    }
}