using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.BurnableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public class BurnableObjectUIHandler : SerializedMonoBehaviour
    {
        [OdinSerialize] private IBurnable _burnable;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Transform _windowParent;
        [SerializeField] private Vector3 _offset;

        private WindowService _windowService;
        private BurnableObjectWindow _burnableObjectWindow;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void Start()
        {
            if (!_burnable.IsBurned)
                return;

            InitializeWindow();
            _burnableObjectWindow.ShowIcon();
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_burnable.IsBurned)
                return;
            
            if (_burnableObjectWindow == null)
                InitializeWindow();
            
            _burnableObjectWindow.ShowIcon();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_burnable.IsBurned)
                return;

            if (_burnableObjectWindow == null)
                InitializeWindow();

            _burnableObjectWindow.ShowButton();
        }

        private void InitializeWindow()
        {
            _burnableObjectWindow = _windowService.GetNew<BurnableObjectWindow>();
            _burnableObjectWindow.transform.SetParent(_windowParent);
            _burnableObjectWindow.transform.position = _windowParent.position + _offset;
            _burnableObjectWindow.Init(_burnable);
            _burnableObjectWindow.Open();
        }
    }
}