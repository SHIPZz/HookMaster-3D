using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using CodeBase.UI.Buttons;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.MiningFarm
{
    public class MiningFarmUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        [SerializeField] private OpenMiningFarmWindowButton _openMiningFarmWindowButton;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
        }
        
        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerApproached;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _openMiningFarmWindowButton.onClick.AddListener(OnButtonClicked);
        }

        private void Start()
        {
            _buttonScaleAnim.UnScale();
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerApproached;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnPlayerExited(Collider obj)
        {
            _buttonScaleAnim.UnScale();
        }

        private void OnPlayerApproached(Collider obj)
        {
            _buttonScaleAnim.ToScale();
            print("scale");
        }

        private void OnButtonClicked()
        {
            _buttonScaleAnim.UnScale();
        }
    }
}