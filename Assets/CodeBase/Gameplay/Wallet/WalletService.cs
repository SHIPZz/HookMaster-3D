using System;
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

        public void AddDiamonds(int diamonds) =>
            UpdateData(WalletValueTypeId.Diamond, diamonds, count => DiamondsChanged?.Invoke(count));

        public void DecreaseDiamonds(int diamonds)
            => UpdateData(WalletValueTypeId.Diamond, -diamonds, count => DiamondsChanged?.Invoke(count));

        public void AddTickets(int tickets) =>
            UpdateData(WalletValueTypeId.Ticket, tickets, count => TicketCountChanged?.Invoke(count));

        public void DecreaseTickets(int tickets) =>
            UpdateData(WalletValueTypeId.Ticket, -tickets, count => TicketCountChanged?.Invoke(count));

        public void AddMoney(int money) =>
            UpdateData(WalletValueTypeId.Money, money, count => MoneyChanged?.Invoke(count));

        public void DecreaseMoney(int money) =>
            UpdateData(WalletValueTypeId.Money, -money, count => MoneyChanged?.Invoke(count));

        public bool HasEnoughMoney(int money) =>
            HasEnoughCount(CurrentMoney, money);

        public bool HasEnoughDiamonds(int diamonds) =>
            HasEnoughCount(CurrentDiamonds, diamonds);

        public bool HasEnoughTickets(int tickets) =>
            HasEnoughCount(CurrentTickets, tickets);

        private bool HasEnoughCount(int target, int count) =>
            target - count >= 0;

        private void UpdateData(WalletValueTypeId walletValueTypeId, int amount, Action<int> onComplete)
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;

            switch (walletValueTypeId)
            {
                case WalletValueTypeId.Money:
                    playerData.Money = Mathf.Clamp(playerData.Money + amount, 0, MaxValueCount);
                    CurrentMoney = playerData.Money;
                    onComplete?.Invoke(CurrentMoney);
                    break;

                case WalletValueTypeId.Ticket:
                    playerData.Tickets = Mathf.Clamp(playerData.Tickets + amount, 0, MaxValueCount);
                    CurrentTickets = playerData.Tickets;
                    onComplete?.Invoke(CurrentTickets);
                    break;

                case WalletValueTypeId.Diamond:
                    playerData.Diamonds = Mathf.Clamp(playerData.Diamonds + amount, 0, MaxValueCount);
                    CurrentDiamonds = playerData.Diamonds;
                    onComplete?.Invoke(CurrentDiamonds);
                    break;
            }
            
            _worldDataService.Save();
        }
    }
}