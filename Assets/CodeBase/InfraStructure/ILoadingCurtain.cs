using System;

namespace CodeBase.InfraStructure
{
    public interface ILoadingCurtain
    {
        event Action Closed;
        void Show(float sliderDuration);
        void Hide();
    }
}