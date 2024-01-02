using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Gameplay.Wallet
{
    public class WalletService
    {
        private const int MaxValueCount = 100000000;
        private readonly IWorldDataService _worldDataService;

        private Dictionary<ItemTypeId, Action<int>> _addAcitons = new();
        private Dictionary<ItemTypeId, Action<int>> _removeAcitons = new();

        public int CurrentMoney { get; private set; }
        public int CurrentTickets { get; private set; }
        public int CurrentDiamonds { get; private set; }

        public event Action<int> MoneyChanged;
        public event Action<int> DiamondsChanged;
        public event Action<int> TicketCountChanged;

        public WalletService(IWorldDataService worldDataService) =>
            _worldDataService = worldDataService;

        public void Init()
        {
            FillDictionaries();

            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            CurrentMoney = playerData.Money;
            CurrentTickets = playerData.Tickets;
            CurrentDiamonds = playerData.Diamonds;
            MoneyChanged?.Invoke(playerData.Money);
            TicketCountChanged?.Invoke(playerData.Tickets);
            DiamondsChanged?.Invoke(playerData.Diamonds);
        }

        public void Add(ItemTypeId itemTypeId, int amount) => 
            _addAcitons[itemTypeId]?.Invoke(amount);

        public void Remove(ItemTypeId itemTypeId, int amount) =>
            _removeAcitons[itemTypeId]?.Invoke(amount);

        public void AddDiamonds(int diamonds) =>
            UpdateData(ItemTypeId.Diamond, diamonds, count => DiamondsChanged?.Invoke(count));

        public void RemoveDiamonds(int diamonds)
            => UpdateData(ItemTypeId.Diamond, -diamonds, count => DiamondsChanged?.Invoke(count));

        public void AddTickets(int tickets) =>
            UpdateData(ItemTypeId.Ticket, tickets, count => TicketCountChanged?.Invoke(count));

        public void RemoveTickets(int tickets) =>
            UpdateData(ItemTypeId.Ticket, -tickets, count => TicketCountChanged?.Invoke(count));

        public void AddMoney(int money) =>
            UpdateData(ItemTypeId.Money, money, count => MoneyChanged?.Invoke(count));

        public void RemoveMoney(int money) =>
            UpdateData(ItemTypeId.Money, -money, count => MoneyChanged?.Invoke(count));

        public bool HasEnoughMoney(int money) =>
            HasEnoughCount(CurrentMoney, money);

        public bool HasEnoughDiamonds(int diamonds) =>
            HasEnoughCount(CurrentDiamonds, diamonds);

        public bool HasEnoughTickets(int tickets) =>
            HasEnoughCount(CurrentTickets, tickets);

        private bool HasEnoughCount(int target, int count) =>
            target - count >= 0;

        private void UpdateData(ItemTypeId itemTypeId, int amount, Action<int> onComplete)
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;

            switch (itemTypeId)
            {
                case ItemTypeId.Money:
                    playerData.Money = Mathf.Clamp(playerData.Money + amount, 0, MaxValueCount);
                    CurrentMoney = playerData.Money;
                    onComplete?.Invoke(CurrentMoney);
                    break;

                case ItemTypeId.Ticket:
                    playerData.Tickets = Mathf.Clamp(playerData.Tickets + amount, 0, MaxValueCount);
                    CurrentTickets = playerData.Tickets;
                    onComplete?.Invoke(CurrentTickets);
                    break;

                case ItemTypeId.Diamond:
                    playerData.Diamonds = Mathf.Clamp(playerData.Diamonds + amount, 0, MaxValueCount);
                    CurrentDiamonds = playerData.Diamonds;
                    onComplete?.Invoke(CurrentDiamonds);
                    break;
            }
            
            _worldDataService.Save();
        }

        private void FillDictionaries()
        {
            _addAcitons[ItemTypeId.Money] = AddMoney;
            _addAcitons[ItemTypeId.Ticket] = AddTickets;
            _addAcitons[ItemTypeId.Diamond] = AddDiamonds;

            _removeAcitons[ItemTypeId.Money] = RemoveMoney;
            _removeAcitons[ItemTypeId.Ticket] = RemoveTickets;
            _removeAcitons[ItemTypeId.Diamond] = RemoveDiamonds;
        }
    }
}