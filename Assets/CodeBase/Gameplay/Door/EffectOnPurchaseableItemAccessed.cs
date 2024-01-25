using System;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.MaterialChanger;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Door
{
    [RequireComponent(typeof(DoorSystem))]
    public class EffectOnPurchaseableItemAccessed : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _targetValue = 1f;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private AppearanceEffect _appearanceEffect;

        private PurchaseableItem _purchaseableItem;
        private RendererMaterialChangerService _rendererMaterialChangerService;
        private bool _isChanged;
        private Collider _targetCollider;

        [Inject]
        private void Construct(RendererMaterialChangerService rendererMaterialChangerService)
        {
            _rendererMaterialChangerService = rendererMaterialChangerService;
        }

        private void Awake()
        {
            _purchaseableItem = GetComponent<PurchaseableItem>();
            _targetCollider = _purchaseableItem.GetComponent<Collider>();
        }

        private void Start()
        {
            _purchaseableItem.AccessChanged += OnAccessChanged;
            var targetRenderer = _purchaseableItem.GetComponent<UnityEngine.Renderer>();
            _rendererMaterialChangerService.Init(_duration, _targetValue, MaterialTypeId.B7, targetRenderer);

            if (_purchaseableItem.IsAсcessible)
                _rendererMaterialChangerService.Change(() => _purchaseableItem.gameObject.SetActive(false));
        }

        private void OnDisable() =>
            _purchaseableItem.AccessChanged -= OnAccessChanged;

        private void OnAccessChanged(bool isAccessed)
        {
            if (_isChanged)
                return;

            if (!isAccessed)
                return;

            _isChanged = true;
            _targetCollider.enabled = false;
            _soundPlayerSystem.PlayActiveSound();
            _appearanceEffect.PlayAppearEffect();
            _rendererMaterialChangerService.Change(() => _purchaseableItem.gameObject.SetActive(false));
        }
    }
}