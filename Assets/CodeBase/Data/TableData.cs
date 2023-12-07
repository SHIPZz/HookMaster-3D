using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class TableData
    {
        public List<Guid> BusyTables = new();
    }
}