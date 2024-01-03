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

        private void OnEnable() =>
            _button.onClick.AddListener(Play);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Play);
        
        public void Play()
        {
            _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // _audioSource.Play();
        }
    }
}