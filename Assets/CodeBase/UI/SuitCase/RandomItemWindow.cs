using CodeBase.Animations;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SuitCase
{
    public class RandomItemWindow : WindowBase
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _profit;
        [SerializeField] private Image _image;
        
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

        public void Init(string name, string profit, Sprite sprite, Vector2 targetPosition)
        {
            _name.text = name;
            _profit.text = profit;
            _image.rectTransform.anchoredPosition = targetPosition;
            _image.sprite = sprite;
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