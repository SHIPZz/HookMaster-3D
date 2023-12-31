﻿using System;
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
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            CurrentMoney = playerData.Money;
            CurrentTickets = playerData.Tickets;
            CurrentDiamonds = playerData.Diamonds;
            MoneyChanged?.Invoke(playerData.Money);
            TicketCountChanged?.Invoke(playerData.Tickets);
            DiamondsChanged?.Invoke(playerData.Diamonds);
        }

        public void Set(ItemTypeId itemTypeId, int amount)
        {
            switch (itemTypeId)
            {
                case ItemTypeId.Money:
                    SetMoney(amount);
                    break;

                case ItemTypeId.Ticket:
                    SetTickets(amount);
                    break;

                case ItemTypeId.Diamond:
                    SetDiamonds(amount);
                    break;
            }
        }

        public bool HasEnough(ItemTypeId itemTypeId, int amount)
        {
            switch (itemTypeId)
            {
                case ItemTypeId.Money:
                    return HasEnoughMoney(amount);

                case ItemTypeId.Ticket:
                    return HasEnoughTickets(amount);

                case ItemTypeId.Diamond:
                    return HasEnoughDiamonds(amount);
            }

            return false;
        }

        private bool HasEnoughMoney(int money) =>
            HasEnoughCount(CurrentMoney, money);

        private bool HasEnoughDiamonds(int diamonds) =>
            HasEnoughCount(CurrentDiamonds, diamonds);

        private bool HasEnoughTickets(int tickets) =>
            HasEnoughCount(CurrentTickets, tickets);

        private bool HasEnoughCount(int target, int count) =>
            target - count >= 0;

        private void SetDiamonds(int diamonds) =>
            UpdateData(ItemTypeId.Diamond, diamonds, count => DiamondsChanged?.Invoke(count));

        private void SetTickets(int tickets) =>
            UpdateData(ItemTypeId.Ticket, tickets, count => TicketCountChanged?.Invoke(count));

        private void SetMoney(int money) =>
            UpdateData(ItemTypeId.Money, money, count => MoneyChanged?.Invoke(count));

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
    }
}