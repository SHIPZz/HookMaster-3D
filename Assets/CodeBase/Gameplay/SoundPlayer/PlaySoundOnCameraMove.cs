using CodeBase.Gameplay.Camera;
using UnityEngine;

namespace CodeBase.Gameplay.SoundPlayer
{
    public class PlaySoundOnCameraMove : MonoBehaviour
    {
        [SerializeField] private CameraFollower _camera;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;

        private void OnEnable() => 
            _camera.Moved += Play;

        private void OnDisable() => 
            _camera.Moved -= Play;

        private void Play() => 
            _soundPlayerSystem.PlayActiveSound();
    }
}