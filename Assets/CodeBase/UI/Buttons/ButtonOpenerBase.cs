using CodeBase.Services.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public abstract class ButtonOpenerBase : Button
    {
        protected WindowService WindowService;

        [Inject]
        private void Construct(WindowService windowService) =>
            WindowService = windowService;

        protected override void Awake() => 
            onClick.AddListener(Open);

        protected override void OnDisable() =>
            onClick.RemoveListener(Open);

        protected abstract void Open();
    }
}