using CodeBase.Gameplay.SoundPlayer;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayFootStepSoundOnAnim : MonoBehaviour
    {
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
    
        public void PlayFootStepSound()
        {
            _soundPlayerSystem.PlayActiveSound();
        }

        public void PlayLeftFoot()
        {
            _soundPlayerSystem.PlayInactiveSound();
        }
    }
}
