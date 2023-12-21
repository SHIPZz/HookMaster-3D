using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundOnButtonClick : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Button _button;
        [SerializeField] private bool _playOnDestroy;

        private void OnEnable() =>
            _button.onClick.AddListener(Play);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Play);

        private void OnDestroy()
        {
            if (_playOnDestroy)
                Play();
        }
        
        public void Play()
        {
            // _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _audioSource.Play();
        }
    }
}