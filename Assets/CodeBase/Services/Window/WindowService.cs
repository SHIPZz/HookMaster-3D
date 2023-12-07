using System;
using System.Collections.Generic;
using CodeBase.Services.Factories.UI;
using CodeBase.UI;
using CodeBase.UI.Employee;
using Unity.VisualScripting;

namespace CodeBase.Services.Window
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;
        private readonly Dictionary<Type, Func<WindowBase>> _windowRegistry = new Dictionary<Type, Func<WindowBase>>();
        private Dictionary<Type, WindowBase> _createdWindows = new();

        public WindowService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            Init();
        }

        private void Init()
        {
            Register<EmployeeWindow>(_uiFactory.CreateEmployeeWindow);
            Register<EmployeeWorkWindow>(_uiFactory.CreateEmployeeWorkWindow);
        }

        public void Open<T>() where T : WindowBase
        {
            if (!_windowRegistry.TryGetValue(typeof(T), out Func<WindowBase> windowCreator))
                return;

            WindowBase targetWindow = windowCreator?.Invoke();
            _createdWindows[typeof(T)] = targetWindow;
            targetWindow.Open();
        }

        public T GetAndOpen<T>() where T : WindowBase
        {
            if (!_windowRegistry.TryGetValue(typeof(T), out Func<WindowBase> windowCreator))
                return null;

            WindowBase targetWindow = windowCreator?.Invoke();
            _createdWindows[typeof(T)] = targetWindow;
            targetWindow.Open();
            return (T)targetWindow;
        }
        
        public void Close<T>() where T : WindowBase
        {
            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase windowBase))
                return;

            if (windowBase != null)
                windowBase.Close();
        }

        private void Register<T>(Func<WindowBase> windowCreator) where T : WindowBase =>
            _windowRegistry[typeof(T)] = windowCreator;
    }
}