using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Wallet
{
    public class WalletService
    {
        private const int MaxValueCount = 10000000;
        private readonly IWorldDataService _worldDataService;

        private Dictionary<ItemTypeId, int> _walletResources;
        private Dictionary<ItemTypeId, Action<int>> _updateDataActions = new();

        public event Action<int> MoneyChanged;
        public event Action<int> DiamondsChanged;
        public event Action<int> TicketCountChanged;

        public WalletService(IWorldDataService worldDataService) =>
            _worldDataService = worldDataService;

        public void Init()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            _updateDataActions[ItemTypeId.Diamond] = OnDiamondsChanged;
            _updateDataActions[ItemTypeId.Money] = OnMoneyChanged;
            _updateDataActions[ItemTypeId.Ticket] = OnTicketCountChanged;

            _walletResources = playerData.WalletResources;
        }

        public int GetValue(ItemTypeId itemTypeId) =>
            _walletResources[itemTypeId];

        public void Set(ItemTypeId itemTypeId, int amount)
        {
            _walletResources[itemTypeId] = Mathf.Clamp(_walletResources[itemTypeId] + amount, 0, MaxValueCount);
            _updateDataActions[itemTypeId]?.Invoke(_walletResources[itemTypeId]);
            UpdateData();
        }

        public bool HasEnough(ItemTypeId itemTypeId, int amount) =>
            _walletResources[itemTypeId] - amount >= 0;

        private void OnMoneyChanged(int amount) => MoneyChanged?.Invoke(amount);
        private void OnTicketCountChanged(int amount) => TicketCountChanged?.Invoke(amount);
        private void OnDiamondsChanged(int amount) => DiamondsChanged?.Invoke(amount);

        private void UpdateData()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            playerData.WalletResources = _walletResources;
        }
    }
}