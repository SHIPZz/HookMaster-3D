using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.TableSystem;
using UnityEngine;

namespace CodeBase.Services.Providers.Tables
{
    public class TableService : MonoBehaviour
    {
        public List<Table> Tables;
        public int AvailableTableCount { get; private set; }

        public event Action AllTablesBusy;
        public event Action TableConditionChanged;

        private void Awake()
        {
            AvailableTableCount = Tables.Count(x => x.IsFree);
        }

        private void OnEnable()
        {
            Tables.ForEach(x => x.ConditionChanged += OnTableConditionChanged);
        }

        private void OnDisable()
        {
            Tables.ForEach(x => x.ConditionChanged -= OnTableConditionChanged);
        }

        private void OnTableConditionChanged(bool isBusy)
        {
            AvailableTableCount = Tables.Count(x => x.IsFree);
            
            if(AvailableTableCount == 0)
                AllTablesBusy?.Invoke();
            
            TableConditionChanged?.Invoke();
        }
    }
}