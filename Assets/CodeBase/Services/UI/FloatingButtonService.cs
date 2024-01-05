using CodeBase.Services.Factories.UI;
using CodeBase.Services.GOPool;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Services.UI
{
    public class FloatingButtonService
    {
        private readonly UIFactory _uiFactory;
        private Button _targetButton;
        private Vector2 _initialAnchoredPosition;
        private readonly ObjectPool<Button, string, Transform> _buttonObjectPool;

        public FloatingButtonService(UIFactory uiFactory)
        {
            _buttonObjectPool = new ObjectPool<Button, string, Transform>(uiFactory.CreateElement<Button>, 1);
        }

        public Button Get() => _targetButton;

        public void ShowFloatingButton(float additionalAnchoredPositionY,
            float duration,
            Quaternion rotation,
            string path, Transform target,
            bool isFadeInCanvas, bool setInitialPosition)
        {
            if (_targetButton == null)
                _targetButton = _buttonObjectPool.Pop(path, target);

            ConfigureButton(rotation);
            var rectTransformAnimator = _targetButton.GetComponent<RectTransformAnimator>();
            var canvasAnimator = _targetButton.GetComponent<CanvasAnimator>();

            if (setInitialPosition)
                rectTransformAnimator.SetInitialPosition();

            ConfigureRectTransformAnimator(rectTransformAnimator, canvasAnimator);
            rectTransformAnimator.MoveAnchoredPositionY(additionalAnchoredPositionY, duration);

            if (!isFadeInCanvas)
                HandleFadeOut(canvasAnimator);
        }

        private void ConfigureButton(Quaternion rotation)
        {
            _targetButton.gameObject.SetActive(true);

            if (rotation != Quaternion.identity)
                _targetButton.transform.rotation = rotation;
        }

        private void ConfigureRectTransformAnimator(RectTransformAnimator rectTransformAnimator,
            CanvasAnimator canvasAnimator)
        {
            rectTransformAnimator.SetRotation(rectTransformAnimator.transform.rotation);

            canvasAnimator.FadeInCanvas();
        }

        private void HandleFadeOut(CanvasAnimator canvasAnimator)
        {
            canvasAnimator.FadeOutCanvas(() => _buttonObjectPool.Push(_targetButton));
        }
    }
}