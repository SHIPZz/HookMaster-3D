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

        public StartHireEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService) 
            : base(uiFactory, windowService, worldDataService) { }

        public override void OnStart()
        {
            WorldDataService.WorldData.CompletedTutorials.TryAdd(typeof(StartHireEmployeeStep).FullName, false);
            WindowService.Opened += OnOpened;
        }

        public override void OnFinished()
        {
            _tutorialFadeImage.gameObject.SetActive(false);
            _tutorialHand.gameObject.SetActive(false);
            _targetButton.onClick.RemoveListener(OnFinished);
            WorldDataService.WorldData.CompletedTutorials[typeof(StartHireEmployeeStep).FullName] = true;
        }
        
        public override bool IsCompleted()
        {
            WorldDataService.WorldData.CompletedTutorials.TryAdd(typeof(StartHireEmployeeStep).FullName, false);
            return WorldDataService.WorldData.CompletedTutorials[typeof(StartHireEmployeeStep).FullName];
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