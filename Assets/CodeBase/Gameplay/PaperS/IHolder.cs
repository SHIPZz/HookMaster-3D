using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.PaperS
{
    public interface IHolder
    {
        int ItemsCount { get; }
        bool CanPut { get; }
        UniTask<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken);
        UniTask PutAsync(IHoldable holdable, CancellationToken cancellationToken);
    }
}