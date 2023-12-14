﻿using DG.Tweening;
using UnityEngine;

public class CanvasAnimator : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.25f;

    private Tween _fadeTween;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public void FadeInCanvas()
    {
        _fadeTween?.Kill(true);
        _canvasGroup.interactable = true;
        _fadeTween = _canvasGroup.DOFade(1f, _fadeInDuration);
    }

    public void FadeOutCanvas()
    {
        _fadeTween?.Kill(true);

        _fadeTween = _canvasGroup.DOFade(0f, _fadeOutDuration)
            .OnComplete(() => _canvasGroup.interactable = false);
    }
}