using System;
using Newtonsoft.Json;

namespace CodeBase.Data
{
    [Serializable]
    public class VectorData
    {
        public float X;
        public float Y;
        public float Z;

        public VectorData(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}