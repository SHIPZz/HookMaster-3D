using System;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeProfitUIHandler : MonoBehaviour
    {
        [SerializeField] private Employee _employee;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private RectTransform _profitTextRectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _additionalAnchoredPositionY = 3f;

        private ProfitService _profitService;
        private CameraProvider _cameraProvider;
        private Vector2 _initialTextAnchoredPosition;

        [Inject]
        private void Construct(ProfitService profitService, CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _profitService = profitService;
        }

        private void Awake() => 
            _initialTextAnchoredPosition = _profitTextRectTransform.anchoredPosition;

        private void OnEnable() => 
            _profitService.ProfitGot += OnProfitGot;

        private void OnDisable() => 
            _profitService.ProfitGot -= OnProfitGot;

        [Button]
        private void OnProfitGot(Guid employeeId, int minuteProfit)
        {
            if (_employee.Guid != employeeId)
                return;

            _profitText.text = minuteProfit.ToString();
            _profitTextRectTransform.DOAnchorPosY(_initialTextAnchoredPosition.y, 0f);
            _canvasGroup.DOFade(1, 0.5f);
            _profitTextRectTransform.rotation = Quaternion.LookRotation(_cameraProvider.Camera.transform.forward);
            _profitTextRectTransform
                .DOAnchorPosY(_initialTextAnchoredPosition.y + _additionalAnchoredPositionY, 2f)
                .OnComplete(()=> _canvasGroup.DOFade(0, 0.5f));
            
        }
    }
}