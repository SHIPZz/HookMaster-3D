using System;
using System.Collections.Generic;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class CouchData
    {
        public string Id;
        public Dictionary<SideTypeId, bool> SideConditions = new();
        public bool IsFree;
    }
}