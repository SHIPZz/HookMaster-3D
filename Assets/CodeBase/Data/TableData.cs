using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeBase.Data
{
    [Serializable]
    public class TableData
    {
       [DataMember] public List<string> BusyTableIds = new();
    }
}