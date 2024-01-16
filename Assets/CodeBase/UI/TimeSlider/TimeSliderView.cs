using CodeBase.Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.TimeSlider
{
    public class TimeSliderView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        public bool IsWorking { get; private set; }
        
        private Tween _tween;
        
        public void SetMaxValue(float maxValue)
        {
            _slider.maxValue = maxValue;
        }

        public void Disable()
        {
            IsWorking = false;
            
            _canvasAnimator.FadeOutCanvas(() =>
            {
                gameObject.SetActive(false);
                _slider.value = 0;
            });
        }

        public void FillToMaxValue(float duration)
        {
            gameObject.SetActive(true);
            _canvasAnimator.FadeInCanvas();
            IsWorking = true;
           _tween = _slider.DOValue(_slider.maxValue, duration);
        }

        public void Stop()
        {
            _tween?.Kill();
            Disable();
        }
        
        public void Fill(float value, float duration)
        {
            _slider.DOValue(value, duration);
        }
    }
}