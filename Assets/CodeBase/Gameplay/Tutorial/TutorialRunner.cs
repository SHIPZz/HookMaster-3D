using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Tutorial;
using CodeBase.Services.Window;
using UnityEngine;
using Zenject;

public class TutorialRunner
{
    private Dictionary<Type, TutorialStep> _tutorialSteps = new();

    private readonly IInstantiator _instantiator;

    public TutorialRunner(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public void Init()
    {
        _tutorialSteps[typeof(StartHireEmployeeStep)] = _instantiator.Instantiate<StartHireEmployeeStep>();
        _tutorialSteps[typeof(HireEmployeeStep)] = _instantiator.Instantiate<HireEmployeeStep>();
        _tutorialSteps[typeof(ApproachToEmployeeStep)] = _instantiator.Instantiate<ApproachToEmployeeStep>();
        _tutorialSteps.Values.ToList().ForEach(x => x.OnStart());
    }
}