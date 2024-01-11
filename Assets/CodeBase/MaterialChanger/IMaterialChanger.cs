using System;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    public interface IMaterialChanger
    {
        void Change();
        public event Action StartedChanged;
        public event Action Completed;
        void SetInitialMaterial();
        bool IsChanging { get; }
    }
}