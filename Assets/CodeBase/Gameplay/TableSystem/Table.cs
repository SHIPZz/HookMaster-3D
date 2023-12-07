using System;
using UnityEngine;

namespace CodeBase.Gameplay.TableSystem
{
    public class Table : MonoBehaviour
    {
        public bool IsFree;

        public Guid Guid { get; private set; }

        public event Action<bool> ConditionChanged;

        private void Awake()
        {
            if (Guid.Equals(Guid.Empty))
            {
                Guid = new Guid();
            }
        }

        public void SetCondition(bool isFree)
        {
            IsFree = isFree;
            ConditionChanged?.Invoke(IsFree);
        }
    }
}