using CodeBase.Animations;
using CodeBase.Gameplay.BurnableObjectSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.BurnableObjects
{
    public class BurnableObjectWindow : WindowBase
    {
        [field: SerializeField] public Button RecoverButton { get; private set; }
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private RectTransformScaleAnim _burnedIconScaleAnim;
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        private IBurnable _burnable;

        private void OnEnable()
        {
            RecoverButton.onClick.AddListener(OnRecoverButtonClicked);
        }

        private void OnDisable()
        {
            RecoverButton.onClick.RemoveListener(OnRecoverButtonClicked);
        }

        public void Init(IBurnable burnable)
        {
            _burnable = burnable;
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
        }

        public void ShowIcon()
        {
            _buttonScaleAnim.UnScale();
            _burnedIconScaleAnim.ToScale();
        }

        public void ShowButton()
        {
            _burnedIconScaleAnim.UnScale();
            _buttonScaleAnim.ToScale();
        }

        private void OnRecoverButtonClicked()
        {
            if (!_burnable.IsBurned)
                return;

            _burnable.Recover();
            _burnedIconScaleAnim.UnScale();
            _buttonScaleAnim.UnScale();
        }
    }
}