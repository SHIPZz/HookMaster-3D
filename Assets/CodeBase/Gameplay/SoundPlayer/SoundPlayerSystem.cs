using UnityEngine;

namespace CodeBase.Gameplay.SoundPlayer
{
    public class SoundPlayerSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource _activeClip;
        [SerializeField] private AudioSource _inactiveClip;

        public void PlayActiveSound() => _activeClip.Play();

        public void PlayInactiveSound() => _inactiveClip.Play();

        public void StopActiveSound() => _activeClip.Stop();
    }
}