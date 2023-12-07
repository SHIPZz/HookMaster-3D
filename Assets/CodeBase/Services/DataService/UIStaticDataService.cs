using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class UIStaticDataService
    {
        private readonly Dictionary<Type, WindowBase> _windows;

        public UIStaticDataService()
        {
            _windows = Resources.LoadAll<WindowBase>("Prefabs/Windows")
                .ToDictionary(x => x.GetType(), x => x);
        }

        public WindowBase Get<T>() where T : WindowBase => 
            _windows[typeof(T)];
    }
}