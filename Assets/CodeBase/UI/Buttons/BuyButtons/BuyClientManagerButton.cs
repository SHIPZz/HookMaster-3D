using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Gameplay.ServiceTables;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Buttons.BuyButtons
{
    public class BuyClientManagerButton : CheckOutButton
    {
        [SerializeField] private TransformScaleAnim _transformScaleAnim;
        [SerializeField] private TMP_Text _priceText;
        
        private ServiceClientTable _serviceClientTable;

        protected override void Awake()
        {
            base.Awake();
            Value = GameConstantValue.ClientManagerCost;
        }

        public void Set(ServiceClientTable serviceClientTable)
        {
            _serviceClientTable = serviceClientTable;
            _priceText.text = $"{GameConstantValue.ClientManagerCost}$";
        }

        public void ToScale()
        {
            _transformScaleAnim.ToScale();
        }

        public void UnScale()
        {
            _transformScaleAnim.UnScale();
        }
        
        protected override void OnClicked()
        {
            base.OnClicked();
            _serviceClientTable.EnableManagerChair();
        }
    }
}