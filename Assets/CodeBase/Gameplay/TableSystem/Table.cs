using System;
using UnityEngine;

namespace CodeBase.Gameplay.TableSystem
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private bool _isFree = true;

        public bool IsFree => _isFree;
        
        public event Action<bool> ConditionChanged;

        public void SetCondition(bool isFree)
        {
            _isFree = isFree;
            ConditionChanged?.Invoke(_isFree);
        }
    }
}