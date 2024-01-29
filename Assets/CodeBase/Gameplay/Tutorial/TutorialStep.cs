﻿using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;

namespace CodeBase.Gameplay.Tutorial
{
    public abstract class TutorialStep
    {
        protected UIFactory UIFactory;
        protected WindowService WindowService;
        protected IWorldDataService WorldDataService;
        protected string ClassName;

        protected TutorialStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService)
        {
            UIFactory = uiFactory;
            WindowService = windowService;
            WorldDataService = worldDataService;
        }

        public abstract void OnStart();
        public abstract void OnFinished();

        public void AddToData()
        {
            ClassName = GetType().FullName;

            if (!WorldDataService.WorldData.TutorialData.CompletedTutorials.ContainsKey(ClassName))
                WorldDataService.WorldData.TutorialData.CompletedTutorials[ClassName] = false;
        }

        protected void SetCompleteToData(bool isCompleted)
        {
            WorldDataService.WorldData.TutorialData.CompletedTutorials[ClassName] = isCompleted;
        }

        protected virtual bool IsCompleted()
        {
            return WorldDataService.WorldData.TutorialData.CompletedTutorials[ClassName];
        }
    }
}