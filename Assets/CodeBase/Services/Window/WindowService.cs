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

        public WindowService(UIFactory uiFactory) => 
            _uiFactory = uiFactory;

        public void Open<T>() where T : WindowBase
        {
            ClearDestroyedWindows();
            
            WindowBase targetWindow = _uiFactory.CreateWindow<T>();
            _createdWindows[typeof(T)] = targetWindow;
            targetWindow.Open();
        }

        public T OpenAndGet<T>() where T : WindowBase
        {
            Open<T>();
            
            return (T)_createdWindows[typeof(T)];
        }

        public void Close<T>() where T : WindowBase
        {
            ClearDestroyedWindows();
            
            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase windowBase))
                return;

            windowBase.Close();
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


        public void CloseAll() =>
            _createdWindows.Values.ToList().ForEach(x => x.Close());
    }
}