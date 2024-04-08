using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.ResourceItem
{
    internal class ResourceCollectionTask : IDisposable
    {
        private readonly Resource _resource;
        private readonly Vector3 _startPoint;
        private readonly Transform _target;
        private readonly Transform _controlPoint;
        private readonly ResourceCollectionSettings _settings;
        private readonly CancellationTokenSource _lifetimeToken;
        

        public ResourceCollectionTask(Resource resource,
            Transform target,
            Transform controlPoint,
            ResourceCollectionSettings settings)
        {
            _resource = resource;
            _startPoint = resource.transform.position;
            _target = target;
            _controlPoint = controlPoint;
            _settings = settings;
            _lifetimeToken = new CancellationTokenSource();
        }

        public async UniTask CompleteAsync()
        {
            float elapsedTime = 0f;
            float timeSlider = 0f;

            while (timeSlider < 1)
            {
                Vector3 pos = Vector3.Lerp(_resource.transform.position, _target.position, timeSlider);

                pos.y += Mathf.Sin(timeSlider * Mathf.PI);
                
                _resource.transform.position = pos;
                
                if (_resource.NeedChangeScale)
                {
                    var sinScale = Mathf.Sin(timeSlider * Mathf.PI);
                    _resource.transform.localScale =
                        Vector3.Lerp(_resource.transform.localScale, _settings.TargetScale, sinScale);
                }

                timeSlider += Time.deltaTime * _settings.FlySpeed;

                await UniTask.Yield();
            }

            _resource.transform.position = _target.position;

            if (_resource.NeedChangeScale)
                _resource.transform.DOScale(0, 0.2f);
        }

        public async UniTask ExecuteAsync(CancellationToken cancellationToken)
        {
            CancellationToken token = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken, _lifetimeToken.Token).Token;

            var timePassed = 0f;

            var curveLength = CalculateCurveLength();

            while (timePassed < 1f)
            {
                token.ThrowIfCancellationRequested();

                var distanceToMove = _settings.FlySpeed * Time.deltaTime;
                timePassed += GetTIncrement(timePassed, distanceToMove, curveLength);
                timePassed = Mathf.Clamp01(timePassed);
                Vector3 position = MoveAlongCurve(timePassed);

                _resource.transform.position = position;

                if (_resource.NeedChangeScale)
                    _resource.transform.localScale = Vector3.Lerp(_resource.transform.localScale, Vector3.zero,
                        _settings.ScaleSpeed * Time.deltaTime);

                await UniTask.Yield();
            }

            if (_resource.NeedChangeScale)
                _resource.transform.localScale = Vector3.zero;
        }

        public void Dispose()
        {
            _lifetimeToken?.Cancel();
            _lifetimeToken?.Dispose();
        }

        private float CalculateCurveLength()
        {
            var length = 0.0f;
            var prevPoint = _startPoint;
            var segments = 100;

            for (var i = 1; i <= segments; i++)
            {
                var t = i / (float)segments;
                Vector3 point = CalculateBezierPoint(t, _startPoint, _controlPoint.position, _target.position);
                length += Vector3.Distance(prevPoint, point);
                prevPoint = point;
            }

            return length;
        }

        private float GetTIncrement(float currentT, float distanceToMove, float curveLength)
        {
            return (distanceToMove / curveLength) / (1 - currentT);
        }

        private Vector3 MoveAlongCurve(float t)
        {
            return CalculateBezierPoint(t, _startPoint, _controlPoint.position, _target.position);
        }

        private Vector3 CalculateBezierPoint(float t,
            Vector3 p0,
            Vector3 p1,
            Vector3 p2)
        {
            // Quadratic Bezier curve formula
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0; //first term
            p += 2 * u * t * p1; //second term
            p += tt * p2; //third term

            return p;
        }
    }
}