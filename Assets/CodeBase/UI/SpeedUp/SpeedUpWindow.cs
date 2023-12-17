using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.SpeedUp
{
    public class SpeedUpWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(base.Close);
        }
    }
}