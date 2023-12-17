using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.InfraStructure
{
    public interface ILoadingCurtain
    {
        event Action Closed;
        void Show(float sliderDuration);
        UniTaskVoid Hide();
    }
}