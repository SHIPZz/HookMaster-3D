using CodeBase.Services.Providers.Location;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.SoundPlayer
{
    public class PlaySoundOnButtonClick : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Button _button;
        [SerializeField] private bool _setParent;
        private LocationProvider _locationProvider;

        [Inject]
        private void Construct(LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Play);
            
            if(_setParent)
                _audioSource.gameObject.transform.SetParent(_locationProvider.AudioParent);
        }

        private void OnDisable() =>
            _button.onClick.RemoveListener(Play);
        
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