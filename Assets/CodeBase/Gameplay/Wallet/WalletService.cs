using System;
using CodeBase.Data;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletService
    {
        private const int MaxMoney = 100000000;
        private readonly IWorldDataService _worldDataService;
        public event Action<int> MoneyChanged;

        public WalletService(IWorldDataService worldDataService) => 
            _worldDataService = worldDataService;

        public void Init()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            MoneyChanged?.Invoke(playerData.Money);
        }
        
        public void Add(int money) => 
            SetMoneyToData(money);

        public void Decrease(int money) => 
            SetMoneyToData(-money);

        private void SetMoneyToData(int money)
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            playerData.Money = Mathf.Clamp(playerData.Money + money, 0, MaxMoney);
            MoneyChanged?.Invoke(playerData.Money);
            _worldDataService.Save();
        }
    }
}