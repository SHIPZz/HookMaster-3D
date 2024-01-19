using CodeBase.Constant;
using CodeBase.Gameplay.Employees;
using CodeBase.Services.Employees;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Upgrade;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Tutorial
{
    public class ApproachToEmployeeStep : TutorialStep
    {
        private readonly EmployeeHirerService _employeeHirerService;
        private readonly Vector3 _spawnOffset = new(0, 1.5f, 0);
        private SpriteRenderer _pointer;

        public ApproachToEmployeeStep(UIFactory uiFactory, WindowService windowService,
            IWorldDataService worldDataService, EmployeeHirerService employeeHirerService)
            : base(uiFactory, windowService, worldDataService)
        {
            _employeeHirerService = employeeHirerService;
        }

        public override void OnStart()
        {
            WorldDataService.WorldData.CompletedTutorials[typeof(ApproachToEmployeeStep).FullName] = false;
            _employeeHirerService.EmployeeHired += OnHired;
            WindowService.Opened += OnWindowOpened;
        }

        public override void OnFinished()
        {
            _pointer.gameObject.SetActive(false);
            WorldDataService.WorldData.CompletedTutorials[typeof(ApproachToEmployeeStep).FullName] = true;
            _employeeHirerService.EmployeeHired -= OnHired;
            WindowService.Opened -= OnWindowOpened;
        }

        public override bool IsCompleted()
        {
            WorldDataService.WorldData.CompletedTutorials.TryAdd(typeof(ApproachToEmployeeStep).FullName, false);
            return WorldDataService.WorldData.CompletedTutorials[typeof(ApproachToEmployeeStep).FullName];
        }

        private void OnWindowOpened(WindowBase window)
        {
            if(IsCompleted())
                return;
            
            if (window.GetType() != typeof(UpgradeEmployeeWindow))
                return;

            OnFinished();
        }

        private async void OnHired(Employee employee)
        {
            if(IsCompleted())
                return;
            
            while (!employee.IsWorking)
            {
                await UniTask.Yield();
            }

            _pointer = UIFactory.CreateElement<SpriteRenderer>(AssetPath.Pointer3D, employee.transform);
            _pointer.transform.position += _spawnOffset;
            _pointer.transform.up = employee.transform.up;
        }
    }
}