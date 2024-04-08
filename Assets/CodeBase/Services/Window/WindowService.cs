using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.Factories.UI;
using CodeBase.UI;
using CodeBase.UI.Hud;
using Sirenix.Utilities;

namespace CodeBase.Services.Window
{
    public class WindowService
    {
        private readonly UIFactory _uiFactory;
        private Dictionary<Type, WindowBase> _createdWindows = new();

        public WindowBase CurrentWindow { get; private set; }

        public event Action<WindowBase> Opened;
        public event Action<WindowBase> HudOpened;

        public WindowService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open<T>() where T : WindowBase
        {
            ClearDestroyedWindows();

            var targetWindow = Get<T>();
            targetWindow.Open();

            if (targetWindow.GetType() == typeof(HudWindow))
            {
                HudOpened?.Invoke(targetWindow);
            }

            Opened?.Invoke(targetWindow);
        }

        public void OpenCurrentWindow()
        {
            CurrentWindow.Open();
            Opened?.Invoke(CurrentWindow);
        }

        public void OpenCreatedWindow<T>() where T : WindowBase
        {
            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase window))
                return;
            
            window.Open();
            Opened?.Invoke(window);
        }

        public T OpenAndGet<T>() where T : WindowBase
        {
            Open<T>();

            return (T)_createdWindows[typeof(T)];
        }

        public T GetNew<T>() where T : WindowBase
        {
            ClearDestroyedWindows();

            WindowBase targetWindow = _uiFactory.CreateWindow<T>();
            CurrentWindow = targetWindow;
            _createdWindows[typeof(T)] = targetWindow;
            return (T)targetWindow;
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

        public T GetWithoutSettingToCurrentWindowAndCaching<T>() where T : WindowBase
        {
            WindowBase targetWindow = _uiFactory.CreateWindow<T>();
            return (T)targetWindow;
        }

        public void CloseAll()
        {
            ClearDestroyedWindows();
            _createdWindows.Values.ForEach(x => x.Close());
        }

        public void Close<T>() where T : WindowBase
        {
            ClearDestroyedWindows();

            if (!_createdWindows.TryGetValue(typeof(T), out WindowBase windowBase))
                return;

            windowBase.Close();
            ClearDestroyedWindows();
        }

        public bool IsWindowOfType<T>() where T : WindowBase =>
            CurrentWindow.GetType() == typeof(T);

        private void ClearDestroyedWindows()
        {
            var windowsToRemove = _createdWindows
                .Where(pair => pair.Value == null)
                .Select(pair => pair.Key)
                .Reverse();

            foreach (Type typeToRemove in windowsToRemove)
                _createdWindows.Remove(typeToRemove);
        }
    }
}