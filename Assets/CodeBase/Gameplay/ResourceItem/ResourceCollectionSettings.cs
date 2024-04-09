using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.ResourceItem
{
    [CreateAssetMenu]
    internal class ResourceCollectionSettings : ScriptableObject
    {
        public float FlySpeed = 5f;
        public float ScaleSpeed = 5f;
        public float Duration = 0.5f;
        public float PutTime = 0.1f;
        public Ease PutEase = Ease.Linear;

        public Vector3 TargetScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}