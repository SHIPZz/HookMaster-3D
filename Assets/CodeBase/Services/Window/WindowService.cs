using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.Factories.UI;
using CodeBase.UI;

namespace CodeBase.Services.Window
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;
        private Dictionary<Type, WindowBase> _createdWindows = new();

        public WindowService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open<T>() where T : WindowBase
        {
            WindowBase targetWindow = _uiFactory.CreateWindow<T>();
            _createdWindows[typeof(T)] = targetWindow;
            targetWindow.Open();
        }

        public void Close<T>() where T : WindowBase
        {
            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase windowBase))
                return;

            if (windowBase != null)
                windowBase.Close();
        }

        public void CloseAll() =>
            _createdWindows.Values.ToList().ForEach(x => x.Close());
    }
}