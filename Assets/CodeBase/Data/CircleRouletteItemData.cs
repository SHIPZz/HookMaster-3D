using System;

namespace CodeBase.Data
{
    [Serializable]
    public class CircleRouletteItemData
    {
        public string Id;
        public int MaxWinValue;
        public int MinWinValue;
        public VectorData Position;
        public int PlayTime;
    }
}