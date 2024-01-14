using UnityEngine;

namespace CodeBase.Gameplay
{
    public class SoundPlayerSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource _activeClip;
        [SerializeField] private AudioSource _inactiveClip;

        public void PlayActiveSound() => 
            _activeClip.Play();

        public void PlayInactiveClip() => 
            _inactiveClip.Play();
    }
}