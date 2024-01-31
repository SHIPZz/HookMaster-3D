using System;
using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Employee;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class HireEmployeeStep : TutorialStep, IDisposable
    {
        private readonly Vector2 _tutorialHandOffset = new Vector2(32.01f, -63.29f);
        private Image _tutorialHand;
        private EmployeeView _employeeView;
        private EmployeeWindow _employeeWindow;

        public HireEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService)
            : base(uiFactory, windowService, worldDataService) { }

        public override async void OnStart()
        {
            if (IsCompleted())
                return;

            WindowService.Opened += OnOpened;
        }

        public override void OnFinished()
        {
            _employeeView.Closed -= OnFinished;
            _tutorialHand.gameObject.SetActive(false);
            _employeeWindow.TutorialFadeImage.gameObject.SetActive(false);
            SetCompleteToData(true);
            WorldDataService.WorldData.TutorialData.CompletedTutorials[ClassName] = true;
        }

        private void OnOpened(WindowBase windowBase)
        {
            if (windowBase.GetType() != typeof(EmployeeWindow) || IsCompleted())
                return;

            _employeeWindow = WindowService.Get<EmployeeWindow>();
            _employeeWindow.TutorialFadeImage.gameObject.SetActive(true);

            _employeeView = _employeeWindow.GetFirstEmployeeView();
            _tutorialHand =
                UIFactory.CreateElement<Image>(AssetPath.TutorialHand, _employeeView.HireButton.gameObject.transform);
            _tutorialHand.rectTransform.anchoredPosition = _tutorialHandOffset;
            _employeeView.Closed += OnFinished;
        }

        public void Dispose()
        {
            if (_employeeView != null)
                _employeeView.Closed -= OnFinished;
            
            WindowService.Opened -= OnOpened;
        }
    }
}