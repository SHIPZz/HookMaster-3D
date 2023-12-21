using CodeBase.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.SO.Sound
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "Gameplay/Sounds/SoundSO")]
    public class SoundSO : SerializedScriptableObject
    {
        public SoundTypeId SoundTypeId;
        public AudioClip Sound;
    }
}