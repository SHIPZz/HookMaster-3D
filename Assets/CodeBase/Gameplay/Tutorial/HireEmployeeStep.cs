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
        private string _className;

        public HireEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService) 
            : base(uiFactory, windowService, worldDataService) { }
        
        public override void OnStart()
        {
            _className = typeof(ApproachToEmployeeStep).FullName;
            
            WorldDataService.WorldData.TutorialData.CompletedTutorials.TryAdd(_className, false);
            WindowService.Opened += OnOpened;
        }

        public override void OnFinished()
        {
            _employeeView.Closed -= OnFinished;
            _tutorialHand.gameObject.SetActive(false);
            _employeeWindow.TutorialFadeImage.gameObject.SetActive(false);
            WorldDataService.WorldData.TutorialData.CompletedTutorials[_className] = true;
        }

        public override bool IsCompleted()
        {
            WorldDataService.WorldData.TutorialData.CompletedTutorials.TryAdd(_className, false);
            return WorldDataService.WorldData.TutorialData.CompletedTutorials[_className];
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