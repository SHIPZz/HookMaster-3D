using CodeBase.Services.Factories.UI;
using CodeBase.Services.GOPool;
using CodeBase.UI.FloatingText;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class FloatingTextService
    {
        private const int TextCount = 10;
        private readonly EnumObjectPool<FloatingTextView, Transform, FloatingTextType> _enumTextPool;

        private Vector2 _initialTextAnchoredPosition;

        public FloatingTextService(UIFactory uiFactory)
        {
            _enumTextPool = new EnumObjectPool<FloatingTextView, Transform, FloatingTextType>(
                uiFactory.CreateFloatingTextView, TextCount);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType,
            Transform parent,
            Vector3 position,
            bool cameraFacing)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            
            targetFloatingTextView.Init(position, _enumTextPool);
            targetFloatingTextView.SetRotationToCamera(cameraFacing);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType, Transform parent,
            Vector3 position,
            float fontSize,
            float width,
            float height,
            float anchoredPos,
            float minRandomAnchoredPosition)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            targetFloatingTextView.Text.fontSize = fontSize;
            targetFloatingTextView.RectTransform.sizeDelta = new Vector2(width, height);

            targetFloatingTextView.Init(position, _enumTextPool);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType, Transform parent, Vector3 position, string text)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            targetFloatingTextView.Text.text = text;
            targetFloatingTextView.Init(position, _enumTextPool);
        }
    }
}