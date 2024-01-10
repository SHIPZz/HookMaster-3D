using System;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.Effects
{
    public class EffectView : MonoBehaviour
    {
        [field: SerializeField] public EffectTypeId EffectTypeId { get; protected set; }
        [field: SerializeField] public ParticleSystem Effect { get; protected set; }
    }
}