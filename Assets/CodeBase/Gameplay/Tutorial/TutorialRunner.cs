using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Tutorial;
using Zenject;

public class TutorialRunner : IDisposable
{
    private readonly IInstantiator _instantiator;

    private Dictionary<Type, TutorialStep> _tutorialSteps = new();

    public TutorialRunner(IInstantiator instantiator) =>
        _instantiator = instantiator;

    public void Init()
    {
        var startHireEmployeeStep = _instantiator.Instantiate<StartHireEmployeeStep>();
        var hireEmployeeStep = _instantiator.Instantiate<HireEmployeeStep>();
        var approachToEmployeeStep = _instantiator.Instantiate<ApproachToEmployeeStep>();
        var upgradeEmployeeStep = _instantiator.Instantiate<UpgradeEmployeeStep>();

        _tutorialSteps[typeof(StartHireEmployeeStep)] = startHireEmployeeStep;
        _tutorialSteps[typeof(HireEmployeeStep)] = hireEmployeeStep;
        _tutorialSteps[typeof(ApproachToEmployeeStep)] = approachToEmployeeStep;
        _tutorialSteps[typeof(UpgradeEmployeeStep)] = upgradeEmployeeStep;

        _tutorialSteps.Values.ToList().ForEach(x =>
        {
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
}