using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

namespace CodeBase.Gameplay.PaperS
{
    public static class TweenExtensions
    {
        public static TweenAwaiter GetAwaiter(this Tween tween)
        {
            return new TweenAwaiter(tween);
        }

        public static async Task PlayAsync(this Tween tween, CancellationToken cancellationToken)
        {
            void Cancel()
            {
                tween.Kill();
            }

            if (cancellationToken.IsCancellationRequested)
            {
                Cancel();
                return;
            }

            CancellationTokenRegistration registration = cancellationToken.Register(Cancel);
            await tween;
            registration.Dispose();
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public class TweenAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }
        private Tween _tween;

        private Action _onComplete;

        public Tween GetResult()
        {
            return _tween;
        }

        public TweenAwaiter(Tween tween)
        {
            _tween = tween;
            _tween.onComplete += OnComplete;
            _tween.onKill += OnComplete;
        }

        private void OnComplete()
        {
            IsCompleted = true;
            _onComplete?.Invoke();
            Dispose();
        }

        private void Dispose()
        {
            _tween.onComplete -= OnComplete;
            _tween.onKill -= OnComplete;
            _tween = null;
            _onComplete = null;
        }

        public void OnCompleted(Action continuation)
        {
            _onComplete = continuation;
        }
    }
}