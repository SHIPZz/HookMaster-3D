using System;
using CodeBase.Data;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletService
    {
        private const float MaxMoney = 100000000;
        private readonly IWorldDataService _worldDataService;
        public event Action<float> MoneyChanged;
        
        public float CurrentMoney { get; private set; }

        public WalletService(IWorldDataService worldDataService) => 
            _worldDataService = worldDataService;

        public void Init()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            CurrentMoney = playerData.Money;
            MoneyChanged?.Invoke(playerData.Money);
        }
        
        public void Add(float money) => 
            SetMoneyToData(money);

        public void Decrease(float money) => 
            SetMoneyToData(-money);

        public bool HasEnoughMoney(float money) => 
            CurrentMoney - money >= 0;

        private void SetMoneyToData(float money)
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            playerData.Money = Mathf.Clamp(playerData.Money + money, 0, MaxMoney);
            CurrentMoney = playerData.Money;
            MoneyChanged?.Invoke(playerData.Money);
            _worldDataService.Save();
        }
    }
}