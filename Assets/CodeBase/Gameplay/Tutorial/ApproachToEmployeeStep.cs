using CodeBase.Constant;
using CodeBase.Extensions;
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
        private EmployeeService _employeeService;
        private string _className;

        public ApproachToEmployeeStep(UIFactory uiFactory, WindowService windowService,
            IWorldDataService worldDataService, EmployeeHirerService employeeHirerService,
            EmployeeService employeeService)
            : base(uiFactory, windowService, worldDataService)
        {
            _employeeService = employeeService;
            _employeeHirerService = employeeHirerService;
        }

        public override void OnStart()
        {
            _className = typeof(ApproachToEmployeeStep).FullName;
            
            if (WorldDataService.WorldData.TutorialData.CompletedTutorials.ContainsKey(_className))
            {
                if(!WorldDataService.WorldData.TutorialData.CompletedTutorials[_className])
                    CreatePointer3D();
            }
            
            _employeeHirerService.EmployeeHired += OnHired;
            WindowService.Opened += OnWindowOpened;
        }

        public override void OnFinished()
        {
            _pointer.gameObject.SetActive(false);
            WorldDataService.WorldData.TutorialData.CompletedTutorials[_className] = true;
            _employeeHirerService.EmployeeHired -= OnHired;
            WindowService.Opened -= OnWindowOpened;
        }

        public override bool IsCompleted()
        {
            WorldDataService.WorldData.TutorialData.CompletedTutorials.TryAdd(_className, false);
            return WorldDataService.WorldData.TutorialData.CompletedTutorials[_className];
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
                await UniTask.Yield();

            _pointer = UIFactory.CreateElement<SpriteRenderer>(AssetPath.Pointer3D, employee.transform);
            _pointer.transform.position += _spawnOffset;
            WorldDataService.WorldData.TutorialData.LastPointerEmployeePosition = _pointer.transform.position.ToData();
            WorldDataService.WorldData.TutorialData.EmployeeId = employee.Id;
            _pointer.transform.up = employee.transform.up;
        }

        private async void CreatePointer3D()
        {
            Vector3 position = WorldDataService.WorldData.TutorialData.LastPointerEmployeePosition.ToVector();

            while (!_employeeService.Initialized) 
                await UniTask.Yield();

            Employee employee = _employeeService.Get(WorldDataService.WorldData.TutorialData.EmployeeId);
            _pointer = UIFactory.CreateElement<SpriteRenderer>(AssetPath.Pointer3D, employee.transform);
            _pointer.transform.position = position;
        }
    }
}