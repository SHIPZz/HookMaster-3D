using Abu;
using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Buttons;
using CodeBase.UI.Hud;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class StartHireEmployeeStep : TutorialStep
    {
        private Image _tutorialHand;
        private OpenEmployeeWindowButton _targetButton;
        private TutorialFadeImage _tutorialFadeImage;
        private bool _onFinished;
        private string _className;

        public StartHireEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService) 
            : base(uiFactory, windowService, worldDataService) { }

        public override void OnStart()
        {
            _className = typeof(ApproachToEmployeeStep).FullName;
            WorldDataService.WorldData.TutorialData.CompletedTutorials.TryAdd(_className, false);
            WindowService.Opened += OnOpened;
        }

        public override void OnFinished()
        {
            _tutorialFadeImage.gameObject.SetActive(false);
            _tutorialHand.gameObject.SetActive(false);
            _targetButton.onClick.RemoveListener(OnFinished);
            WorldDataService.WorldData.TutorialData.CompletedTutorials[_className] = true;
        }
        
        public override bool IsCompleted()
        {
            WorldDataService.WorldData.TutorialData.CompletedTutorials.TryAdd(_className, false);
            return WorldDataService.WorldData.TutorialData.CompletedTutorials[_className];
        }

        private void OnOpened(WindowBase windowBase)
        {
            if (windowBase.GetType() != typeof(HudWindow) || IsCompleted())
                return;

            var hudWindow = WindowService.Get<HudWindow>();
            _tutorialFadeImage = hudWindow.TutorialFadeImage;
            _tutorialFadeImage.gameObject.SetActive(true);
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand, hudWindow.TutorialHandParent);
            _targetButton = hudWindow.OpenEmployeeWindowButton;
            _targetButton.onClick.AddListener(OnFinished);
        }
    }
}