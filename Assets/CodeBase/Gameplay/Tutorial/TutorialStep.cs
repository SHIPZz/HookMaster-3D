using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;

namespace CodeBase.Gameplay.Tutorial
{
    public abstract class TutorialStep
    {
        protected UIFactory UIFactory;
        protected WindowService WindowService;
        protected IWorldDataService WorldDataService;

        protected TutorialStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService)
        {
            UIFactory = uiFactory;
            WindowService = windowService;
            WorldDataService = worldDataService;
        }

        public abstract void OnStart();
        public abstract void OnFinished();

        public abstract bool IsCompleted();
    }
}