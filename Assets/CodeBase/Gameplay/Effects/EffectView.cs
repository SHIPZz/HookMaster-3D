using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.Effects
{
    public class EffectView : MonoBehaviour
    {
        [field: SerializeField] public EffectTypeId EffectTypeId { get; private set; }
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
    }
}