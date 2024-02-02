using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.PaperSystem;
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
        private List<TableData> _tableDatas;
        public int AvailableTableCount { get; private set; }
        public int AllTableCount => Tables.Count;

        public event Action AllTablesBusy;
        public event Action TableConditionChanged;

        [Inject]
        private void Construct(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        private void OnDisable()
        {
            Tables.ForEach(x => x.Busy -= OnTableConditionChanged);
        }

        public void Init(List<TableData> tableDatas)
        {
            _tableDatas = tableDatas;

            if (tableDatas.Count == 0)
                tableDatas.AddRange(Tables.Select(table => table.ToData()));

            SetTableValuesFromData();
            
            AvailableTableCount = Tables.Count(x => x.IsFree);
            Tables.ForEach(x =>
            {
                x.Init();
                x.Busy += OnTableConditionChanged;
            });
        }

        public PaperTable Get(string id)
        {
            var table = Tables.FirstOrDefault(x => x.Id == id);
            return table.GetComponent<PaperTable>();
        } 

        public void RecoverTableFromBurning(string id)
        {
            Tables.FirstOrDefault(x => x.Id == id)?.Recover();
        }

        private void OnTableConditionChanged(bool isBusy, string id)
        {
            AvailableTableCount = Tables.Count(x => x.IsFree);

            if (AvailableTableCount == 0)
                AllTablesBusy?.Invoke();

            Table table = Tables.FirstOrDefault(x => x.Id == id);
            
            if (_tableDatas.RemoveAll(x=>x.Id == id) != 0)
            {
                _tableDatas.Add(table.ToData());
                _worldDataService.WorldData.TableDatas = _tableDatas;
                _worldDataService.Save();
            }

            TableConditionChanged?.Invoke();
        }

        private void SetTableValuesFromData()
        {
            foreach (var table in Tables)
            {
                TableData tableData = _tableDatas.Find(data => data.Id == table.Id);

                if (tableData != null)
                {
                    table.IsFree = tableData.IsFree;
                    table.IsBurned = tableData.IsBurned;
                }
            }
        }
    }
}