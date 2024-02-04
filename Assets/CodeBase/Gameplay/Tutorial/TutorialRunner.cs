using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace CodeBase.Gameplay.Tutorial
{
    public class TutorialRunner : IDisposable
    {
        private readonly IInstantiator _instantiator;

        private Dictionary<Type, TutorialStep> _tutorialSteps = new();

        public TutorialRunner(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public void Init()
        {
            CreateStep<ApproachToPaperTableStep>();
            CreateStep<StartHireEmployeeStep>();
            CreateStep<HireEmployeeStep>();
            CreateStep<ApproachToEmployeeStep>();
            CreateStep<UpgradeEmployeeStep>();
            CreateStep<ShowClientServeRoomStep>();
            
            _tutorialSteps.Values.ToList().ForEach(x =>
            {
                x.SetTutorialRunner(this);
                x.AddToData();
                x.OnStart();
            });
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _tutorialSteps.Values.Select(tutorialStep => tutorialStep as IDisposable))
            {
                disposable?.Dispose();
            }
        }

        public bool IsTutorialFinished<T>() where T : TutorialStep
        {
            return _tutorialSteps[typeof(T)].IsFinished;
        }

        private void CreateStep<T>() where T : TutorialStep
        {
            var step = _instantiator.Instantiate<T>();
            _tutorialSteps[typeof(T)] = step;
        }
    }
}