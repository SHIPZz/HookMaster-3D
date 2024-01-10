using System.Collections;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.UI;

public class PutOutWindow : WindowBase
{
    [SerializeField] private Button _putOutButton;
    [SerializeField] private CanvasAnimator _canvasAnimator;
    [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;

    private ExtinguisherSystem _extinguisherSystem;

    public override void Open()
    {
        _canvasAnimator.FadeInCanvas();
        _putOutButton.onClick.AddListener(PutOut);
    }

    public override void Close()
    {
        _putOutButton.onClick.RemoveListener(PutOut);
        base.Close();
    }

    public void Init(ExtinguisherSystem extinguisherSystem) =>
        _extinguisherSystem = extinguisherSystem;

    private void PutOut()
    {
        _buttonScaleAnim.UnScale();
        _putOutButton.interactable = false;
        _extinguisherSystem.Activate(() =>
        {
            _buttonScaleAnim.ToScale();
            _putOutButton.interactable = true;
        });
    }
}