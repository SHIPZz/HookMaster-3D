using System;

namespace CodeBase.Data
{
    [Serializable]
    public class TableData
    {
        public string Id;
        public bool IsFree = true;
        public bool IsBurned;
    }
}