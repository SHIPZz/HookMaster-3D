using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.CameraServices
{
    [Serializable]
    public class FocusInfo
    {
        public Transform Target;
        public Func<UniTask<bool>> CanReleaseAsync;
        public Func<bool> CanRelease;
    }
}