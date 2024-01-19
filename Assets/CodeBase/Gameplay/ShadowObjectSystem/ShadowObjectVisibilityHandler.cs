using System;
using CodeBase.Animations;
using CodeBase.Gameplay.PurchaseableSystem;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ShadowObjectSystem
{
    public class ShadowObjectVisibilityHandler : MonoBehaviour
    {
        [SerializeField] private PurchaseableItem _purchaseableItem;
        [SerializeField] private ShadowObject _shadowObject;
        [SerializeField] private float _fadeOutDuration = 1f;
        
        private MaterialFadeAnimService _materialFadeAnimService;

        [Inject]
        private void Construct(MaterialFadeAnimService materialFadeAnimService) => 
            _materialFadeAnimService = materialFadeAnimService;

        private void OnEnable() => 
            _purchaseableItem.AccessChanged += OnAccessed;

        private void OnDisable() => 
            _purchaseableItem.AccessChanged -= OnAccessed;

        private void OnAccessed(bool isAccessed)
        {
            if(!isAccessed)
                return;
            
            _materialFadeAnimService.FadeOut(_shadowObject.Material,_fadeOutDuration, 0);

        }
    }
}