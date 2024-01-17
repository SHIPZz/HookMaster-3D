using System;
using CodeBase.Gameplay.GameItems;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.MiningFarm
{
    [RequireComponent(typeof(Button))]
    public class CleanMiningFarmButton : MonoBehaviour
    {
         private MiningFarmItem _miningFarmItem;
        
        private Button _button;

        private void OnEnable() =>
            _button.onClick.AddListener(Clean);

        private void OnDisable() =>
            _button.onClick.RemoveListener(Clean);

        public void SetMiningFarm(MiningFarmItem miningFarmItem) =>
            _miningFarmItem = miningFarmItem;

        private void Clean()
        {
            _miningFarmItem.SetNeedClean(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}