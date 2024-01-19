using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Employee;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class HireEmployeeStep : TutorialStep
    {
        private Image _tutorialHand;
        private Vector2 _tutorialHandOffset = new Vector2(32.01f, -63.29f);
        private EmployeeView _employeeView;
        private EmployeeWindow _employeeWindow;

        public HireEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService) 
            : base(uiFactory, windowService, worldDataService) { }
        
        public override void OnStart()
        {
            WorldDataService.WorldData.CompletedTutorials.TryAdd(typeof(HireEmployeeStep).FullName, false);
            WindowService.Opened += OnOpened;
        }

        public override void OnFinished()
        {
            _employeeView.Closed -= OnFinished;
            _tutorialHand.gameObject.SetActive(false);
            _employeeWindow.TutorialFadeImage.gameObject.SetActive(false);
            WorldDataService.WorldData.CompletedTutorials[typeof(HireEmployeeStep).FullName] = true;
        }

        public override bool IsCompleted()
        {
            WorldDataService.WorldData.CompletedTutorials.TryAdd(typeof(HireEmployeeStep).FullName, false);
            return WorldDataService.WorldData.CompletedTutorials[typeof(HireEmployeeStep).FullName];
        }

        private void OnOpened(WindowBase windowBase)
        {
            if(windowBase.GetType() != typeof(EmployeeWindow) || IsCompleted())
                return;

            _employeeWindow = WindowService.Get<EmployeeWindow>();
            _employeeWindow.TutorialFadeImage.gameObject.SetActive(true);
            
            _employeeView = _employeeWindow.GetFirstEmployeeView();
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand, _employeeView.HireButton.gameObject.transform);
            _tutorialHand.rectTransform.DOAnchorPos(_tutorialHandOffset, 0);
            _employeeView.Closed += OnFinished;
        }
    }
}