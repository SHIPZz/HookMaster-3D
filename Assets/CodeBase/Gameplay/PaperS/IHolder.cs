using System.Threading;
using System.Threading.Tasks;
using _Project_legacy.Scripts.Papers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.PaperS
{
    public interface IHolder
    {
        int ItemsCount { get; }
        UniTask<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken);
        UniTask PutAsync(IHoldable holdable, CancellationToken cancellationToken);
    }
}