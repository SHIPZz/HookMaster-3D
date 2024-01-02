using CodeBase.Gameplay.Wallet;

namespace CodeBase.Services.CheckOuts
{
    public class CheckOutService
    {
        private WalletService _walletService;

        public bool TryUpgradeEmployee(int price)
        {
            if (!_walletService.HasEnoughMoney(price))
                return false;
            
            _walletService.RemoveMoney(price);
            return true;
        }
    }
}