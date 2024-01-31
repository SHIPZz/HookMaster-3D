using System;
using System.Collections.Generic;
using Abu;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CodeBase.Gameplay.Tutorial
{
    public class TutorialContainer : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<string, Transform> _pointerTransforms = new();
        public TutorialFadeImage TutorialFadeImage;

        public Transform Get<T>() where T : TutorialStep =>
            _pointerTransforms[typeof(T).Name];
    }
}