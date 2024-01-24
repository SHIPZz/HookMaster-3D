using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.UI;
using CodeBase.UI.FloatingText;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.BurnableObjects
{
    public class BurnableObjectWindow : WindowBase
    {
        [field: SerializeField] public Button RecoverButton { get; private set; }
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private RectTransformScaleAnim _burnedIconScaleAnim;
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        [SerializeField] private TMP_Text _buttonText;
        
        private IBurnable _burnable;
        private WalletService _walletService;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(WalletService walletService,FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _walletService = walletService;
        }

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
            _buttonText.text = $"RECOVER {GameConstantValue.RecoverCost}$\n";
        }

        private void OnRecoverButtonClicked()
        {
            if (!_walletService.HasEnough(ItemTypeId.Money, GameConstantValue.RecoverCost))
            {
                _floatingTextService.ShowFloatingText(FloatingTextType.NotEnoughMoney,transform,transform.position,
                    0.31f,1,1,4f, 0f);
                return;
            }
            
            if (!_burnable.IsBurned)
                return;

            _burnable.Recover();
            _burnedIconScaleAnim.UnScale();
            _buttonScaleAnim.UnScale();
        }
    }
}