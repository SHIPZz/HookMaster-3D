using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.SO.Settings;
using CodeBase.UI;
using CodeBase.UI.FloatingText;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class UIStaticDataService
    {
        private readonly Dictionary<Type, WindowBase> _windows;
        private readonly Dictionary<FloatingTextType, FloatingTextView> _floatingTextViews;
        private readonly SettingSO _settingSo;
        
        public UIStaticDataService()
        {
            _windows = Resources.LoadAll<WindowBase>(AssetPath.Windows)
                .ToDictionary(x => x.GetType(), x => x);

            _floatingTextViews = Resources.LoadAll<FloatingTextView>(AssetPath.FloatingTexts)
                .ToDictionary(x => x.FloatingTextType, x => x);

            _settingSo = Resources.Load<SettingSO>(AssetPath.Settings);
        }

        public SettingSO GetSettingSo() =>
            _settingSo;

        public WindowBase Get<T>() where T : WindowBase =>
            _windows[typeof(T)];

        public FloatingTextView Get(FloatingTextType floatingTextType) =>
            _floatingTextViews[floatingTextType];
    }
}