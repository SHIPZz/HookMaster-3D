using System;
using System.Collections.Generic;
using CodeBase.Services.Factories.UI;
using CodeBase.UI;

namespace CodeBase.Services.Window
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;
        private readonly Dictionary<Type, Func<WindowBase>> _windowRegistry = new Dictionary<Type, Func<WindowBase>>();

        public WindowService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            Init();
        }

        private void Init()
        {
            Register<EmployeeWindow>(_uiFactory.CreateEmployeeWindow);
        }

        public void Open<T>() where T : WindowBase
        {
            if(_windowRegistry.TryGetValue(typeof(T), out Func<WindowBase> windowCreator)) 
                windowCreator?.Invoke().Open();
        }

        private void Register<T>(Func<WindowBase> windowCreator) where T : WindowBase => 
            _windowRegistry[typeof(T)] = windowCreator;
    }

}