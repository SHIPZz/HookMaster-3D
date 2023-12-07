using System;
using UnityEngine;

namespace CodeBase.Gameplay.TableSystem
{
    public class Table : MonoBehaviour
    {
        public bool IsFree;

        public string Id;
        
        public event Action<bool, string> ConditionChanged;

        [ContextMenu("CreateGuid")]
        public void CreateGuid()
        {
            var guid = Guid.NewGuid();
            Id = guid.ToString();
        }

        public void SetCondition(bool isFree)
        {
            IsFree = isFree;
            ConditionChanged?.Invoke(IsFree, Id);
        }
    }
}