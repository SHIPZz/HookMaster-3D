using CodeBase.Animations;
using CodeBase.Services.TriggerObserve;
using CodeBase.UI.Buttons;
using UnityEngine;

namespace CodeBase.UI.MiningFarm
{
    public class MiningFarmUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        [SerializeField] private OpenMiningFarmWindowButton _openMiningFarmWindowButton;
        
        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerApproached;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _openMiningFarmWindowButton.onClick.AddListener(OnPlayClicked);
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
        }

        private void OnPlayClicked()
        {
            _buttonScaleAnim.UnScale();
        }
    }
}