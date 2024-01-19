using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Mining;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Mining
{
    [RequireComponent(typeof(Button))]
    public class CleanMiningFarmButton : MonoBehaviour
    {
        private MiningFarmItem _miningFarmItem;
        private Button _button;
        private MiningFarmService _miningFarmService;

        [Inject]
        private void Construct(MiningFarmService miningFarmService) => 
            _miningFarmService = miningFarmService;

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable() =>
            _button.onClick.AddListener(Clean);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Clean);

        public void SetMiningFarm(MiningFarmItem miningFarmItem) =>
            _miningFarmItem = miningFarmItem;

        private void Clean()
        {
            _miningFarmService.SetNeedClean(_miningFarmItem.Id, false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}