using System;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.UI;
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

        private ProfitService _profitService;
        private CameraProvider _cameraProvider;
        private Vector2 _initialTextAnchoredPosition;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(ProfitService profitService,
            CameraProvider cameraProvider,
            [Inject(Id = ColorTypeId.Money)] Color moneyColor,
            FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _cameraProvider = cameraProvider;
            _profitService = profitService;
        }

        private void OnEnable() =>
            _profitService.ProfitGot += OnProfitGot;

        private void OnDisable() =>
            _profitService.ProfitGot -= OnProfitGot;

        [Button]
        private void OnProfitGot(Guid employeeId, int minuteProfit)
        {
            if (_employee.Guid != employeeId)
                return;
            
            _floatingTextService
                .ShowFloatingText($"{minuteProfit}$", 
                     _additionalAnchoredPositionY,
                    _moveTextDuration, _fadeInDuration, _fadeOutDuration,
                    Quaternion.LookRotation(_cameraProvider.Camera.transform.forward),
                    AssetPath.ProfitText, _employee.transform);
        }

    }
}