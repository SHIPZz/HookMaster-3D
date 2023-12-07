using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldTimeData
    {
        public Date CurrentTime = new();
        public Date LastVisitedTime = new();
    }

    [Serializable]
    public class Date
    {
        public int Year;
        public int Month;
        public int Day;
    }
}