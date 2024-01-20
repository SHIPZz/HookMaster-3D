using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class TutorialData
    {
        public Dictionary<string, bool> CompletedTutorials = new();
        public VectorData LastPointerEmployeePosition;
        public string EmployeeId;
    }
}