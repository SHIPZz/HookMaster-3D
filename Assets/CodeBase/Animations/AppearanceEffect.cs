using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Animations
{
    public class AppearanceEffect : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _loopEffects;
        [SerializeField] private ParticleSystem _appearEffect;

        public void PlayLoopEffects() =>
            _loopEffects.ForEach(x => x.Play());

        public void PlayAppearEffect() => _appearEffect.Play();
    }
}