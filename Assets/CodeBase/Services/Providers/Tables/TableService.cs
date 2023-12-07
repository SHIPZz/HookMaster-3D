using System;
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

        public void Init(List<string> busyTables)
        {
            foreach (Table table in Tables)
            {
                print(busyTables.Count);
                bool isFree = busyTables.Contains(table.Id);

                table.SetCondition(!isFree);
            }

            AvailableTableCount = Tables.Count(x => x.IsFree);
            Tables.ForEach(x => x.ConditionChanged += OnTableConditionChanged);
            AllTableCount = Tables.Count;
        }

        private void OnDisable() =>
            Tables.ForEach(x => x.ConditionChanged -= OnTableConditionChanged);

        private void OnTableConditionChanged(bool isBusy, string id)
        {
            AvailableTableCount = Tables.Count(x => x.IsFree);

            if (AvailableTableCount == 0)
                AllTablesBusy?.Invoke();

            if (!_worldDataService.WorldData.TableData.BusyTableIds.Contains(id))
            {
                _worldDataService.WorldData.TableData.BusyTableIds.Add(id);
                _worldDataService.Save();
            }

            TableConditionChanged?.Invoke();
        }
    }
}