using CodeBase.Services.GOPool;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Effects
{
    public class SmokeEffectView : EffectView
    {
        [SerializeField] private float _disableDelay = 2.5f;
        private EffectPool _effectPool;

        public void Play()
        {
            Effect.Play(true);
            
            DOTween.Sequence().AppendInterval(_disableDelay).OnComplete(() =>
            {
                Effect.Stop();
                _effectPool.Push(this);
            });
        }
        
        public void SetPool(EffectPool effectPool)
        {
            _effectPool = effectPool;
        }
        
    }
}