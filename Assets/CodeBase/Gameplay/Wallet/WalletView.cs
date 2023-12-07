using TMPro;
using UnityEngine;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;

        public void SetMoney(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}