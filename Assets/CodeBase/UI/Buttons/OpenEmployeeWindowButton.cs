using CodeBase.Services.Window;
using CodeBase.UI.Employee;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class OpenEmployeeWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        
        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService) =>
            _windowService = windowService;

        private void Awake() =>
            _openButton.onClick.AddListener(Open);

        private void OnDisable() =>
            _openButton.onClick.RemoveListener(Open);

        private void Open() =>
            _windowService.Open<EmployeeWindow>();
    }
}