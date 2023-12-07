using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button _closeButton;

        private void Awake() =>
            _closeButton.onClick.AddListener(Close);

        public abstract void Open();
        public abstract void Close();
    }
}