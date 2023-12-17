using CodeBase.Services.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public abstract class ButtonOpenerBase : MonoBehaviour
    {
        [SerializeField] protected Button OpenButton;
        
        protected WindowService WindowService;

        [Inject]
        private void Construct(WindowService windowService) =>
            WindowService = windowService;

        protected virtual void Awake() =>
            OpenButton.onClick.AddListener(Open);

        protected virtual void OnDisable() =>
            OpenButton.onClick.RemoveListener(Open);

        protected abstract void Open();
    }
}