using System.Threading;
using System.Threading.Tasks;
using _Project_legacy.Scripts.Papers;
using UnityEngine;

namespace CodeBase.Gameplay.PaperS
{
    public interface IHolder
    {
        int ItemsCount { get; }
        Task<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken);
        Task PutAsync(IHoldable holdable, CancellationToken cancellationToken);
    }
}