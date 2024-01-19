﻿using System;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using CodeBase.Services.Employees;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Employees
{
    public class Employee : MonoBehaviour, IBurnable
    {
        [field: SerializeField] public MaterialTypeId BurnMaterial { get; private set; }
        [SerializeField] private UnityEngine.Renderer _renderer;
        [field: SerializeField] public bool IsBurned { get; set; }

        public Guid Guid;
        public string Id;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public string Name;
        public string TableId;
        public bool IsWorking;
        public bool IsUpgrading;
        
        private bool _wasWorking;
        private EmployeeDataService _employeeDataService;
        private BurnableObjectService _burnableObjectService;
        private RendererMaterialChangerService _rendererMaterialChangerService;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService, BurnableObjectService burnableObjectService,
            RendererMaterialChangerService rendererMaterialChangerService)
        {
            _rendererMaterialChangerService = rendererMaterialChangerService;
            _burnableObjectService = burnableObjectService;
            _employeeDataService = employeeDataService;
        }

        private void Start()
        {
            _burnableObjectService.Add(this);
            _rendererMaterialChangerService.Init(1.5f, 1f, BurnMaterial, _renderer);

            if (IsBurned)
                Burn();
        }

        public void StartWorking()
        {
            IsWorking = true;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void SetUpgrading(bool isUpgrading)
        {
            IsUpgrading = isUpgrading;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void StopWorking()
        {
            IsWorking = false;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void Burn()
        {
            _wasWorking = IsWorking;
            IsBurned = true;
            _rendererMaterialChangerService.Change();
            StopWorking();
        }

        [Button]
        public void Recover()
        {
            IsBurned = false;
            _rendererMaterialChangerService.SetInitialMaterial();

            if (_wasWorking)
                StartWorking();
        }
    }
}