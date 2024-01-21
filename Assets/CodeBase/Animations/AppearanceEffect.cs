using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Animations
{
    public class AppearanceEffect : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _loopEffects;
        [SerializeField] private ParticleSystem _appearEffect;
        [SerializeField] private List<ParticleSystem> _targetEffects;

        public void PlayLoopEffects() =>
            _loopEffects.ForEach(x => x.Play());

        public void PlayAppearEffect() => _appearEffect.Play();

        public void PlayTargetEffects() => _targetEffects.ForEach(x => x.Play());
    }
}