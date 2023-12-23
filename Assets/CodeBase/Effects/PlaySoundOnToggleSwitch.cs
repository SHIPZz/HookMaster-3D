using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Effects
{
    public class PlaySoundOnToggleSwitch : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private AudioSource _audioSource;

        private void OnEnable() =>
            _toggle.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(bool arg0)
        {
            _audioSource.Play();
        }
    }
}