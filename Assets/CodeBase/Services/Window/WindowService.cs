﻿using System;
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

        public WindowService(UIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void Open<T>() where T : WindowBase
        {
            ClearDestroyedWindows();

            var targetWindow = Get<T>();
            targetWindow.Open();
        }

        public T OpenAndGet<T>() where T : WindowBase
        {
            Open<T>();

            return (T)_createdWindows[typeof(T)];
        }

        public T Get<T>() where T : WindowBase
        {
            WindowBase targetWindow = _uiFactory.CreateWindow<T>();

            DestroyIfAlreadyHas<T>();
            
            _createdWindows[typeof(T)] = targetWindow;
            return (T)targetWindow;
        }

        private void DestroyIfAlreadyHas<T>() where T : WindowBase
        {
            if (!_createdWindows.ContainsKey(typeof(T))) 
                return;
            
            WindowBase createdWindow = _createdWindows[typeof(T)];
            createdWindow.Close();
            _createdWindows.Remove(typeof(T));
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