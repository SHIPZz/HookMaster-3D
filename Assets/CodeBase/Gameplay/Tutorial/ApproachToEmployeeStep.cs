﻿using System;
using CodeBase.Constant;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;
using CodeBase.Services.CameraServices;
using CodeBase.Services.Employees;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Upgrade;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Tutorial
{
    public class ApproachToEmployeeStep : TutorialStep, IDisposable
    {
        private readonly EmployeeHirerService _employeeHirerService;
        private readonly Vector3 _spawnOffset = new(0, 2.5f, 0);
        private readonly EmployeeService _employeeService;
        private SpriteRenderer _pointer;
        private bool _pointerCreated;
        private PlayerProvider _playerProvider;

        public ApproachToEmployeeStep(UIFactory uiFactory,
            WindowService windowService,
            IWorldDataService worldDataService,
            EmployeeHirerService employeeHirerService,
            EmployeeService employeeService,
            PlayerProvider playerProvider) : base(uiFactory, windowService, worldDataService)
        {
            _playerProvider = playerProvider;
            _employeeService = employeeService;
            _employeeHirerService = employeeHirerService;
        }

        public override async void OnStart()
        {
            if (IsCompleted())
                return;

            _employeeHirerService.EmployeeHired += OnHired;
            WindowService.Opened += OnWindowOpened;

            if (WorldDataService.WorldData.TutorialData.LastPointerEmployeePosition != null)
            {
                await UniTask.WaitUntil(() => _playerProvider.PlayerPaperContainer.HasPapers);
                TryCreatePointer3D();
            }
        }

        public override void OnFinished()
        {
            _pointer?.gameObject.SetActive(false);
            SetCompleteToData(true);
            UnSubscribe();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        private void UnSubscribe()
        {
            _employeeHirerService.EmployeeHired -= OnHired;
            WindowService.Opened -= OnWindowOpened;
        }

        private void OnWindowOpened(WindowBase window)
        {
            if (IsCompleted())
                return;

            if (window.GetType() != typeof(UpgradeEmployeeWindow))
                return;

            OnFinished();
        }

        private async void OnHired(Employee employee)
        {
            if (_pointerCreated)
                return;

            if (IsCompleted())
                return;

            await UniTask.WaitUntil(() => employee.IsWorking);
            await UniTask.WaitUntil(() => _playerProvider.PlayerPaperContainer.HasPapers);

            _pointer = UIFactory.CreateElement<SpriteRenderer>(AssetPath.Pointer3D, employee.transform);
            _pointer.transform.position += _spawnOffset;
            WorldDataService.WorldData.TutorialData.LastPointerEmployeePosition = _pointer.transform.position.ToData();
            WorldDataService.WorldData.TutorialData.EmployeeId = employee.Id;
            _pointer.transform.up = employee.transform.up;
            _pointerCreated = true;
        }

        private void TryCreatePointer3D()
        {
            Vector3 position = WorldDataService.WorldData.TutorialData.LastPointerEmployeePosition.ToVector();

            Employee employee = _employeeService.Get(WorldDataService.WorldData.TutorialData.EmployeeId);

            if (employee.IsBurned)
                return;

            _pointer = UIFactory.CreateElement<SpriteRenderer>(AssetPath.Pointer3D, employee.transform);
            _pointer.transform.position = position;
        }
    }
}