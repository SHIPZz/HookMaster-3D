using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button CloseButton;

        private void Awake()
        {
            if (CloseButton != null)
                CloseButton.onClick.AddListener(Close);
        }

        public abstract void Open();

        public virtual void Close()
        {
            Destroy(gameObject);
        }

        public virtual void Show()
        {
            
        }

        public virtual void Hide()
        {
            
        }
    }
}