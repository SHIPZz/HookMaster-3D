using CodeBase.Animations;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.SuitCase
{
    public class SuitCaseWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }
        
        public override void Open()
        {
            _windowService.Close<HudWindow>();
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(()=>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }
    }
}