﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Providers.Tables
{
    public class TableService : MonoBehaviour
    {
        public List<Table> Tables;
        private IWorldDataService _worldDataService;
        public int AvailableTableCount { get; private set; }
        public int AllTableCount { get; private set; }

        public event Action AllTablesBusy;
        public event Action TableConditionChanged;

        [Inject]
        private void Construct(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void Init(List<Guid> busyTables)
        {
            foreach (Table table in Tables)
            {
                table.ConditionChanged += OnTableConditionChanged;
                print(busyTables.Count);
                bool isFree = !busyTables.Contains(table.Guid);

                table.SetCondition(isFree);
                table.IsFree = isFree;

                print(isFree);
            }

            AvailableTableCount = Tables.Count(x => x.IsFree);
            AllTableCount = Tables.Count;
        }
        
        private void OnDisable() =>
            Tables.ForEach(x => x.ConditionChanged -= OnTableConditionChanged);

        private void OnTableConditionChanged(bool isBusy, Guid id)
        {
            AvailableTableCount = Tables.Count(x => x.IsFree);

            if (AvailableTableCount == 0)
                AllTablesBusy?.Invoke();

            _worldDataService.WorldData.TableData.BusyTables.Add(id);
            _worldDataService.Save();
            TableConditionChanged?.Invoke();
        }
    }
}