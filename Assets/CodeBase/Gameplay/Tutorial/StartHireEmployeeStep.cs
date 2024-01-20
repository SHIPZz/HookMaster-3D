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
            if ( IsCompleted() &&  WindowService.CurrentWindow.GetType() != typeof(HudWindow))
                return;
            
            var hudWindow = WindowService.Get<HudWindow>();
            _tutorialFadeImage = hudWindow.TutorialFadeImage;
            _tutorialFadeImage.gameObject.SetActive(true);
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand, hudWindow.TutorialHandParent);
            _targetButton = hudWindow.OpenEmployeeWindowButton;
            _targetButton.onClick.AddListener(OnFinished);
        }

        public override void OnFinished()
        {
            _tutorialFadeImage.gameObject.SetActive(false);
            _tutorialHand.gameObject.SetActive(false);
            _targetButton.onClick.RemoveListener(OnFinished);
            SetCompleteToData(true);
        }
    }
}