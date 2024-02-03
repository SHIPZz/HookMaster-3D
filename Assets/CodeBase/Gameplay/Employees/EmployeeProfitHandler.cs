using System;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Tables;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Employees
{
    public class EmployeeProfitHandler : MonoBehaviour
    {
        [SerializeField] private Employee _employee;

        private ResourceCreator _resourceCreator;
        private TableService _tableService;
        private EmployeeProfitService _employeeProfitService;

        [Inject]
        private void Construct(EmployeeProfitService employeeProfitService, TableService tableService)
        {
            _employeeProfitService = employeeProfitService;
            _tableService = tableService;
        }

        private void Start()
        {
            _employeeProfitService.ProfitGot += OnProfitGot;

            Table targetTable = _tableService.Get(_employee.TableId);
            _resourceCreator = targetTable.ResourceCreator;
        }

        private void OnDisable()
        {
            _employeeProfitService.ProfitGot -= OnProfitGot;
        }

        private void OnProfitGot(string id, int money)
        {
            if (_employee.Id != id)
                return;

            _resourceCreator.Create(Random.Range(5, 15));
        }
    }
}