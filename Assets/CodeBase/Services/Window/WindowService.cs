using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.Factories.UI;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Window
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;
        private Dictionary<Type, WindowBase> _createdWindows = new();
        
        public WindowBase CurrentWindow { get; private set; }

        public event Action<WindowBase> Opened; 

        public WindowService(UIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void Open<T>() where T : WindowBase
        {
            ClearDestroyedWindows();

            var targetWindow = Get<T>();
            targetWindow.Open();
            Opened?.Invoke(targetWindow);
        }

        public T OpenAndGet<T>() where T : WindowBase
        {
            Open<T>();

            return (T)_createdWindows[typeof(T)];
        }

        public T Get<T>() where T : WindowBase
        {
            ClearDestroyedWindows();
            
            if (_createdWindows.ContainsKey(typeof(T)))
            {
                CurrentWindow = (T)_createdWindows[typeof(T)];
                return (T)CurrentWindow;
            }

            WindowBase targetWindow = _uiFactory.CreateWindow<T>();
            CurrentWindow = targetWindow;
            _createdWindows[typeof(T)] = targetWindow;
            return (T)targetWindow;
        }

        public void CloseAll()
        {
            ClearDestroyedWindows();
            _createdWindows.Values.ToList().ForEach(x => x.Close());
        }

        public void Close<T>() where T : WindowBase
        {
            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase windowBase))
                return;

            windowBase.Close();
            ClearDestroyedWindows();
        }

        private void ClearDestroyedWindows()
        {
            List<Type> windowsToRemove = _createdWindows
                .Where(pair => pair.Value == null)
                .Select(pair => pair.Key)
                .ToList();

            foreach (Type typeToRemove in windowsToRemove)
                _createdWindows.Remove(typeToRemove);
        }
    }
}