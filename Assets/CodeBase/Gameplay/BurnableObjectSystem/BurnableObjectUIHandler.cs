using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.BurnableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public class BurnableObjectUIHandler : SerializedMonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Transform _windowParent;
        [SerializeField] private Vector3 _offset;

        private IBurnable _burnable;
        private WindowService _windowService;
        private BurnableObjectWindow _burnableObjectWindow;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        private void Awake()
        {
            _burnable = GetComponent<IBurnable>();
        }

        private void Start()
        {
            if (!_burnable.IsBurned)
                return;

            InitializeWindow();
        }

        private void OnEnable()
        {
            _burnable.Burned += Show;
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
            _burnable.Burned -= Show;
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_burnable.IsBurned)
                return;

            _burnableObjectWindow?.Show();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_burnable.IsBurned)
                return;

            if (_burnableObjectWindow == null)
                InitializeWindow();

            _burnableObjectWindow.ShowButton();
        }

        private void Show(IBurnable burnable)
        {
            if (_burnableObjectWindow == null)
                InitializeWindow();
        }

        private void InitializeWindow()
        {
            _burnableObjectWindow = _windowService.GetWithoutSettingToCurrentWindowAndCaching<BurnableObjectWindow>();
            _burnableObjectWindow.transform.SetParent(_windowParent);
            _burnableObjectWindow.transform.position = _windowParent.position + _offset;
            _burnableObjectWindow.Init(_burnable);
            _burnableObjectWindow.Open();
        }
    }
}