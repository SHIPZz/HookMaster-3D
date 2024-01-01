using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Animations
{
    public class ImageFadeAnim : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _fadeOutDuration = 0.1f;
        [SerializeField] private Image _image;

        private Tween _tween;
        
        public void FadeIn()
        {
            _tween?.Kill(true);
            _image.gameObject.SetActive(true);
            _tween = _image.DOFade(1f, _fadeInDuration);
        }

        public void FadeOut()
        {
            _tween?.Kill(true);

            _tween = _image.DOFade(0f, _fadeOutDuration)
                .OnComplete(() => _image.gameObject.SetActive(false));
        }
    }
}