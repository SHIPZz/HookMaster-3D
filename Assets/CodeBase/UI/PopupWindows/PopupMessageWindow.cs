using System.Collections.Generic;
using CodeBase.Animations;
using DG.Tweening;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.PopupWindows
{
    public class PopupMessageWindow : WindowBase
    {
        [OdinSerialize] private Dictionary<PopupMessageType, Sprite> _messageIcons;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        public void Init(PopupMessageType popupMessageType, string text)
        {
            _image.sprite = _messageIcons[popupMessageType];
            _text.text = text;
        }

        public void SetAutoDestroy(float time) => 
            DOTween.Sequence().AppendInterval(time).OnComplete(Close);

        public override void Open() => 
            _canvasAnimator.FadeInCanvas();

        public override void Close() => 
            _canvasAnimator.FadeOutCanvas(() => base.Close());
    }
}