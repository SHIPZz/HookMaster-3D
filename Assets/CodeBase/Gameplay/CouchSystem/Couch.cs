using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.Gameplay.AccessibleSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Gameplay.CouchSystem
{
    public class Couch : SerializedMonoBehaviour, IAccessible
    {
        public string Id;
        public bool IsFree;
        [field: SerializeField] public bool IsAccessed { get; private set; } = true;
        public Dictionary<SideTypeId, Transform> Sides;
        public Dictionary<SideTypeId, bool> SideConditions;


        public bool HasFreeSide(out Transform targetTransform)
        {
            targetTransform = null;
            
            foreach (KeyValuePair<SideTypeId, bool> keyValuePair in SideConditions.Where(keyValuePair => keyValuePair.Value))
            {
                targetTransform = Sides[keyValuePair.Key];
                SideConditions[keyValuePair.Key] = false;
                IsFree = false;
                return true;
            }

            return false;
        }

        [Button]
        private void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void UnLock() => 
            IsAccessed = true;

        public void Block() => 
            IsAccessed = false;
    }
}