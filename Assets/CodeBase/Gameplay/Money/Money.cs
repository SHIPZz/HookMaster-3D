using System;
using CodeBase.Gameplay.GameItems;
using UnityEngine;

namespace CodeBase.Gameplay.Money
{
    public class Money : GameItemAbstract
    {
        [field: SerializeField] public int Value { get; private set; }
        
        private MoneyMovement _moneyMovement;

        private void Awake()
        {
            _moneyMovement = GetComponent<MoneyMovement>();
        }

        public void Move()
        {
            _moneyMovement.Move();
        }
    }
}